using Microsoft.AspNetCore.Mvc;
using Infrastructure;
using Microsoft.AspNetCore.Http;

namespace MvcSample.Controllers
{
    public class ConfiguracionController : Controller
    {
        private readonly AppDbContext _context;

        public ConfiguracionController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");
            if (rol != "Administrador")
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }
    }
}

