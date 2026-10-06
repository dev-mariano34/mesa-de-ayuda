using System.ComponentModel.DataAnnotations;
using MesaDeAyuda.Api.Models;

namespace MesaDeAyuda.Api.Dtos;

public record IncidenciaDto(
    int Id,
    string Titulo,
    string Descripcion,
    string Estado,
    string Prioridad,
    DateTime FechaCreacion,
    DateTime? FechaActualizacion,
    DateTime? FechaResolucion,
    int? TecnicoAsignadoId,
    string? TecnicoAsignadoNombre);

public class CrearIncidenciaDto
{
    [Required, StringLength(150, MinimumLength = 5)]
    public string Titulo { get; set; } = string.Empty;

    [Required, StringLength(2000, MinimumLength = 10)]
    public string Descripcion { get; set; } = string.Empty;

    public PrioridadIncidencia Prioridad { get; set; } = PrioridadIncidencia.Media;
}

public class ActualizarIncidenciaDto
{
    [Required, StringLength(150, MinimumLength = 5)]
    public string Titulo { get; set; } = string.Empty;

    [Required, StringLength(2000, MinimumLength = 10)]
    public string Descripcion { get; set; } = string.Empty;

    public PrioridadIncidencia Prioridad { get; set; }
}

public class CambiarEstadoDto
{
    [Required]
    public EstadoIncidencia Estado { get; set; }
}

public class AsignarTecnicoDto
{
    public int? TecnicoId { get; set; }
}

public record UsuarioDto(int Id, string Nombre, string Email, string Rol);
