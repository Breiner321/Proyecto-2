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

            // Ignorar temporalmente las propiedades FechaHoraInicio y FechaHoraFin
            // hasta que se agreguen las columnas a la base de datos
            // Esto permite que el código funcione incluso si las columnas no existen
            modelBuilder.Entity<Solicitud>()
                .Ignore(s => s.FechaHoraInicio)
                .Ignore(s => s.FechaHoraFin);
            
            modelBuilder.Entity<SolicitudEquipo>()
                .Ignore(s => s.FechaHoraInicio)
                .Ignore(s => s.FechaHoraFin);

            // Configurar ReporteDano
            modelBuilder.Entity<ReporteDano>()
                .HasOne<Sala>()
                .WithMany()
                .HasForeignKey(r => r.SalaId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

}
