namespace MesaDeAyuda.Api.Models;

public class Incidencia
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public EstadoIncidencia Estado { get; set; } = EstadoIncidencia.Abierta;
    public PrioridadIncidencia Prioridad { get; set; } = PrioridadIncidencia.Media;

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime? FechaActualizacion { get; set; }
    public DateTime? FechaResolucion { get; set; }

    public int? TecnicoAsignadoId { get; set; }
    public Usuario? TecnicoAsignado { get; set; }
}
