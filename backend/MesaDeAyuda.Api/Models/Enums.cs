namespace MesaDeAyuda.Api.Models;

public enum EstadoIncidencia
{
    Abierta = 0,
    EnProgreso = 1,
    Resuelta = 2,
    Cerrada = 3
}

public enum PrioridadIncidencia
{
    Baja = 0,
    Media = 1,
    Alta = 2,
    Critica = 3
}

public enum RolUsuario
{
    Usuario = 0,
    Tecnico = 1,
    Administrador = 2
}
