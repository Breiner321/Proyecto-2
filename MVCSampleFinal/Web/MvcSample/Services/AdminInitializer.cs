using Domain;
using Infrastructure;

namespace MvcSample.Services
{
    public static class AdminInitializer
    {
        public static void InitializeAdmin(AppDbContext context)
        {
            // Verificar si ya existe un administrador
            if (context.Usuarios.Any(u => u.Rol == "Administrador"))
            {
                return; // Ya existe un administrador
            }

            // Crear usuario administrador por defecto
            var admin = new Usuario
            {
                Id = Guid.NewGuid(),
                Nombre = "admin",
                Correo = "admin@usc.edu.co",
                Contraseña = "Admin123",
                Rol = "Administrador"
            };

            context.Usuarios.Add(admin);
            context.SaveChanges();
        }
    }
}

