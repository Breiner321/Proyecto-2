using Microsoft.AspNetCore.Mvc;
using Domain; // tu namespace real
using System.Collections.Generic;
using System.Linq;

namespace MVCSampleFinal.Controllers
{
    public class AuthController : Controller
    {
        private readonly List<Usuario> _usuarios = new List<Usuario>
        {
            new Usuario { Nombre = "admin", Contraseña = "1234" }
        };

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var usuario = _usuarios
                .FirstOrDefault(u => u.Nombre == username && u.Contraseña == password);

            if (usuario == null)
            {
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
