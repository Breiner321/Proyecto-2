using Microsoft.AspNetCore.Mvc;
using MvcSample.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace MvcSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
           
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Redirigir según el rol
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

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
