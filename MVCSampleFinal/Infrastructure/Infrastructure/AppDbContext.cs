using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Sala> Salas { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Solicitud> Solicitudes { get; set; }
        public DbSet<SolicitudEquipo> SolicitudesEquipo { get; set; }
        public DbSet<ReporteDano> ReportesDano { get; set; }
        public DbSet<SolicitudAsesoria> SolicitudesAsesoria { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar la relación Solicitud -> Sala sin navegación para evitar ciclos de cascada
            // La FK se mantiene pero sin propiedad de navegación, y con NoAction para evitar el ciclo
            modelBuilder.Entity<Solicitud>()
                .HasOne<Sala>()
                .WithMany()
                .HasForeignKey(s => s.SalaId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configurar ReporteDano
            modelBuilder.Entity<ReporteDano>()
                .HasOne<Sala>()
                .WithMany()
                .HasForeignKey(r => r.SalaId)
                .OnDelete(DeleteBehavior.NoAction);
            
            // Configurar relaciones de ReporteDano con Usuario y Equipo
            modelBuilder.Entity<ReporteDano>()
                .HasOne(r => r.Usuario)
                .WithMany()
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<ReporteDano>()
                .HasOne(r => r.Equipo)
                .WithMany()
                .HasForeignKey(r => r.EquipoId)
                .OnDelete(DeleteBehavior.NoAction);
            
            // Configurar relaciones de SolicitudAsesoria con Usuario
            modelBuilder.Entity<SolicitudAsesoria>()
                .HasOne(s => s.Usuario)
                .WithMany()
                .HasForeignKey(s => s.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

}
