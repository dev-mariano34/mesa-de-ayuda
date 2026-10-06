using MesaDeAyuda.Api.Data;
using MesaDeAyuda.Api.Dtos;
using MesaDeAyuda.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MesaDeAyuda.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IncidenciasController : ControllerBase
{
    private readonly AppDbContext _db;

    public IncidenciasController(AppDbContext db) => _db = db;

    /// <summary>Lista incidencias, con filtros opcionales por estado y prioridad.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IncidenciaDto>>> Listar(
        [FromQuery] EstadoIncidencia? estado,
        [FromQuery] PrioridadIncidencia? prioridad)
    {
        var query = _db.Incidencias.AsNoTracking().Include(i => i.TecnicoAsignado).AsQueryable();

        if (estado.HasValue) query = query.Where(i => i.Estado == estado.Value);
        if (prioridad.HasValue) query = query.Where(i => i.Prioridad == prioridad.Value);

        var incidencias = await query.OrderByDescending(i => i.FechaCreacion).ToListAsync();
        return Ok(incidencias.Select(ADto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<IncidenciaDto>> Obtener(int id)
    {
        var incidencia = await _db.Incidencias.AsNoTracking()
            .Include(i => i.TecnicoAsignado)
            .FirstOrDefaultAsync(i => i.Id == id);

        return incidencia is null ? NotFound() : Ok(ADto(incidencia));
    }

    [HttpPost]
    public async Task<ActionResult<IncidenciaDto>> Crear(CrearIncidenciaDto dto)
    {
        var incidencia = new Incidencia
        {
            Titulo = dto.Titulo.Trim(),
            Descripcion = dto.Descripcion.Trim(),
            Prioridad = dto.Prioridad
        };

        _db.Incidencias.Add(incidencia);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(Obtener), new { id = incidencia.Id }, ADto(incidencia));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, ActualizarIncidenciaDto dto)
    {
        var incidencia = await _db.Incidencias.FindAsync(id);
        if (incidencia is null) return NotFound();

        incidencia.Titulo = dto.Titulo.Trim();
        incidencia.Descripcion = dto.Descripcion.Trim();
        incidencia.Prioridad = dto.Prioridad;
        incidencia.FechaActualizacion = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id:int}/estado")]
    public async Task<IActionResult> CambiarEstado(int id, CambiarEstadoDto dto)
    {
        var incidencia = await _db.Incidencias.FindAsync(id);
        if (incidencia is null) return NotFound();

        if (incidencia.Estado == EstadoIncidencia.Cerrada)
            return BadRequest(new { mensaje = "Una incidencia cerrada no puede cambiar de estado." });

        incidencia.Estado = dto.Estado;
        incidencia.FechaActualizacion = DateTime.UtcNow;
        incidencia.FechaResolucion = dto.Estado is EstadoIncidencia.Resuelta or EstadoIncidencia.Cerrada
            ? incidencia.FechaResolucion ?? DateTime.UtcNow
            : null;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id:int}/asignar")]
    public async Task<IActionResult> AsignarTecnico(int id, AsignarTecnicoDto dto)
    {
        var incidencia = await _db.Incidencias.FindAsync(id);
        if (incidencia is null) return NotFound();

        if (dto.TecnicoId.HasValue)
        {
            var esTecnico = await _db.Usuarios.AnyAsync(u =>
                u.Id == dto.TecnicoId.Value &&
                (u.Rol == RolUsuario.Tecnico || u.Rol == RolUsuario.Administrador));

            if (!esTecnico)
                return BadRequest(new { mensaje = "El usuario indicado no existe o no es técnico." });

            if (incidencia.Estado == EstadoIncidencia.Abierta)
                incidencia.Estado = EstadoIncidencia.EnProgreso;
        }

        incidencia.TecnicoAsignadoId = dto.TecnicoId;
        incidencia.FechaActualizacion = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var incidencia = await _db.Incidencias.FindAsync(id);
        if (incidencia is null) return NotFound();

        _db.Incidencias.Remove(incidencia);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private static IncidenciaDto ADto(Incidencia i) => new(
        i.Id, i.Titulo, i.Descripcion, i.Estado.ToString(), i.Prioridad.ToString(),
        i.FechaCreacion, i.FechaActualizacion, i.FechaResolucion,
        i.TecnicoAsignadoId, i.TecnicoAsignado?.Nombre);
}
