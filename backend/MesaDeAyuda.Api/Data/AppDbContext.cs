using MesaDeAyuda.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MesaDeAyuda.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Incidencia> Incidencias => Set<Incidencia>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(e =>
        {
            e.Property(u => u.Nombre).HasMaxLength(100).IsRequired();
            e.Property(u => u.Email).HasMaxLength(150).IsRequired();
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Rol).HasConversion<string>().HasMaxLength(20);
        });

        modelBuilder.Entity<Incidencia>(e =>
        {
            e.Property(i => i.Titulo).HasMaxLength(150).IsRequired();
            e.Property(i => i.Descripcion).HasMaxLength(2000).IsRequired();
            e.Property(i => i.Estado).HasConversion<string>().HasMaxLength(20);
            e.Property(i => i.Prioridad).HasConversion<string>().HasMaxLength(20);

            e.HasOne(i => i.TecnicoAsignado)
             .WithMany(u => u.IncidenciasAsignadas)
             .HasForeignKey(i => i.TecnicoAsignadoId)
             .OnDelete(DeleteBehavior.SetNull);

            e.HasIndex(i => i.Estado);
        });

        // Datos iniciales para desarrollo
        modelBuilder.Entity<Usuario>().HasData(
            new Usuario { Id = 1, Nombre = "Administrador", Email = "admin@mesadeayuda.local", Rol = RolUsuario.Administrador },
            new Usuario { Id = 2, Nombre = "Técnico Soporte 1", Email = "tecnico1@mesadeayuda.local", Rol = RolUsuario.Tecnico },
            new Usuario { Id = 3, Nombre = "Técnico Soporte 2", Email = "tecnico2@mesadeayuda.local", Rol = RolUsuario.Tecnico }
        );
    }
}
