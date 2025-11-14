using Microsoft.AspNetCore.Mvc;
using Domain;
using Infrastructure;  // para AppDbContext
using System.Linq;

namespace MVCSampleFinal.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        // El contexto llega por inyección de dependencias
        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Nombre == username && u.Contraseña == password);

            if (usuario == null)
            {
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View();
            }

            // Aquí podrías guardar info en sesión, claims, etc.
            // HttpContext.Session.SetString("Usuario", usuario.Nombre);
            return RedirectToAction("Index", "Home");
        }

        // 👉 NUEVO: Registro de usuario
        [HttpPost]
        public IActionResult Register(string newUsername, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(newUsername) ||
                string.IsNullOrWhiteSpace(newPassword) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                ViewBag.RegisterError = "Todos los campos son obligatorios.";
                return View("Login");
            }

            if (newPassword != confirmPassword)
            {
                ViewBag.RegisterError = "Las contraseñas no coinciden.";
                return View("Login");
            }

            // ¿Ya existe?
            bool existe = _context.Usuarios.Any(u => u.Nombre == newUsername);
            if (existe)
            {
                ViewBag.RegisterError = "El nombre de usuario ya existe.";
                return View("Login");
            }

            var nuevoUsuario = new Usuario
            {
                Nombre = newUsername,
                Contraseña = newPassword  // en real sería mejor hashearla
            };

            _context.Usuarios.Add(nuevoUsuario);
            _context.SaveChanges();  // 👉 se guarda en tu BD en la nube

            ViewBag.RegisterSuccess = "Usuario registrado correctamente. Ahora puede iniciar sesión.";
            return View("Login");
        }
    }
}
