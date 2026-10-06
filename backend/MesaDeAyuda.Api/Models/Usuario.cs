namespace MesaDeAyuda.Api.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public RolUsuario Rol { get; set; } = RolUsuario.Usuario;

    public ICollection<Incidencia> IncidenciasAsignadas { get; set; } = new List<Incidencia>();
}
