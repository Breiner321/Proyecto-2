using Microsoft.AspNetCore.Mvc;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

        // Verificar que el usuario sea administrador
        private bool IsAdministrator()
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");
            return rol == "Administrador";
        }

        // GET: Salas
        public IActionResult Index()
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("Login", "Auth");
            }

            return View(_context.Salas.Include(s => s.Equipos).ToList());
        }

        // GET: Salas/Create
        public IActionResult Create()
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        // POST: Salas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Numero,Capacidad,Ubicacion,Estado")] Sala sala)
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (ModelState.IsValid)
            {
                sala.Id = Guid.NewGuid();
                _context.Add(sala);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(sala);
        }

        // GET: Salas/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id == null)
            {
                return NotFound();
            }

            var sala = _context.Salas.Find(id);
            if (sala == null)
            {
                return NotFound();
            }

            return View(sala);
        }

        // POST: Salas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,Numero,Capacidad,Ubicacion,Estado")] Sala sala)
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id != sala.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sala);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaExists(sala.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(sala);
        }

        // GET: Salas/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id == null)
            {
                return NotFound();
            }

            var sala = _context.Salas
                .Include(s => s.Equipos)
                .FirstOrDefault(m => m.Id == id);

            if (sala == null)
            {
                return NotFound();
            }

            return View(sala);
        }

        // POST: Salas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("Login", "Auth");
            }

            var sala = _context.Salas
                .Include(s => s.Equipos)
                .FirstOrDefault(s => s.Id == id);

            if (sala != null)
            {
                // Verificar si tiene equipos asociados
                if (sala.Equipos != null && sala.Equipos.Any())
                {
                    TempData["Error"] = "No se puede eliminar la sala porque tiene equipos asociados. Elimine primero los equipos.";
                    return RedirectToAction(nameof(Delete), new { id });
                }

                _context.Salas.Remove(sala);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SalaExists(Guid id)
        {
            return _context.Salas.Any(e => e.Id == id);
        }
    }
}
