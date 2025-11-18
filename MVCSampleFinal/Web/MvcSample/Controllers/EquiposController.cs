using Microsoft.AspNetCore.Mvc;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MvcSample.Controllers
{
    public class EquiposController : Controller
    {
        private readonly AppDbContext _context;

        public EquiposController(AppDbContext context)
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

            return View(_context.Equipos.Include(e => e.Sala).ToList());
        }
    }
}

