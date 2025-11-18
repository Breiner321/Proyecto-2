using Microsoft.AspNetCore.Mvc;
using Domain;
using Infrastructure;
using MvcSample.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace MVCSampleFinal.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Si ya está autenticado, redirigir según el rol
            if (HttpContext.Session.GetString("UsuarioId") != null)
            {
                var rol = HttpContext.Session.GetString("UsuarioRol");
                if (rol == "Administrador")
                {
                    return RedirectToAction("Index", "Usuarios");
                }
                else if (rol == "Coordinador")
                {
                    return RedirectToAction("Equipos", "Coordinator");
                }
                else if (rol == "Estudiante" || rol == "Usuario")
                {
                    return RedirectToAction("ReservarSala", "Student");
                }
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Nombre == model.Usuario && u.Contraseña == model.Contraseña);

            if (usuario == null)
            {
                ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                return View(model);
            }

            // Guardar información en sesión
            HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
            HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
            HttpContext.Session.SetString("UsuarioRol", usuario.Rol ?? "Usuario");
            HttpContext.Session.SetString("UsuarioCorreo", usuario.Correo ?? "");

            // Si marcó recordar contraseña, guardar en cookie
            if (model.RecordarContraseña)
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(30),
                    HttpOnly = true,
                    IsEssential = true
                };
                Response.Cookies.Append("UsuarioRecordado", usuario.Nombre, cookieOptions);
            }

            // Redirigir según el rol
            if (usuario.Rol == "Administrador")
            {
                return RedirectToAction("Index", "Usuarios");
            }
            else if (usuario.Rol == "Coordinador")
            {
                return RedirectToAction("Equipos", "Coordinator");
            }
            else if (usuario.Rol == "Estudiante" || usuario.Rol == "Usuario")
            {
                return RedirectToAction("ReservarSala", "Student");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registro(RegistroViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Verificar si el usuario ya existe
            if (_context.Usuarios.Any(u => u.Nombre == model.Nombre))
            {
                ModelState.AddModelError("Nombre", "El nombre de usuario ya existe.");
                return View(model);
            }

            // Verificar si el correo ya existe
            if (_context.Usuarios.Any(u => u.Correo == model.Correo))
            {
                ModelState.AddModelError("Correo", "El correo electrónico ya está registrado.");
                return View(model);
            }

            var nuevoUsuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Nombre = model.Nombre,
                Correo = model.Correo,
                Contraseña = model.Contraseña, // En producción, debería hashearse
                Rol = model.Rol
            };

            _context.Usuarios.Add(nuevoUsuario);
            _context.SaveChanges();

            TempData["RegistroExitoso"] = "Usuario registrado correctamente. Ahora puede iniciar sesión.";
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete("UsuarioRecordado");
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult OlvideContraseña()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OlvideContraseña(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
            {
                ViewBag.Error = "Por favor ingrese su correo electrónico.";
                return View();
            }

            var usuario = _context.Usuarios.FirstOrDefault(u => u.Correo == correo);
            if (usuario == null)
            {
                // Por seguridad, no revelamos si el correo existe o no
                ViewBag.Success = "Si el correo existe, se enviará un enlace para restablecer la contraseña.";
                return View();
            }

            // Aquí deberías implementar el envío de correo para restablecer contraseña
            // Por ahora, solo mostramos un mensaje
            ViewBag.Success = "Se ha enviado un enlace a su correo electrónico para restablecer la contraseña.";
            return View();
        }
    }
}
