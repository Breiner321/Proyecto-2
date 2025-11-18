using Microsoft.AspNetCore.Mvc;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace MvcSample.Controllers
{
    public class SalasController : Controller
    {
        private readonly AppDbContext _context;

        public SalasController(AppDbContext context)
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

            return View(_context.Salas.ToList());
        }
    }
}

