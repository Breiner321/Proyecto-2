using Microsoft.AspNetCore.Mvc;
using Infrastructure;
using Microsoft.AspNetCore.Http;

namespace MvcSample.Controllers
{
    public class ReportesController : Controller
    {
        private readonly AppDbContext _context;

        public ReportesController(AppDbContext context)
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

