using MesaDeAyuda.Api.Data;
using MesaDeAyuda.Api.Dtos;
using MesaDeAyuda.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MesaDeAyuda.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _db;

    public UsuariosController(AppDbContext db) => _db = db;

    /// <summary>Lista los técnicos disponibles para asignar incidencias.</summary>
    [HttpGet("tecnicos")]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> ListarTecnicos()
    {
        var tecnicos = await _db.Usuarios.AsNoTracking()
            .Where(u => u.Rol == RolUsuario.Tecnico || u.Rol == RolUsuario.Administrador)
            .OrderBy(u => u.Nombre)
            .Select(u => new UsuarioDto(u.Id, u.Nombre, u.Email, u.Rol.ToString()))
            .ToListAsync();

        return Ok(tecnicos);
    }
}
