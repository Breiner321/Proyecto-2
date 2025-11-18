using Microsoft.AspNetCore.Mvc;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcSample.Controllers
{
    public class EquiposController : Controller
    {
        private readonly AppDbContext _context;

        public EquiposController(AppDbContext context)
        {
            _context = context;
        }

        // Verificar que el usuario sea administrador
        private bool IsAdministrator()
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");
            return rol == "Administrador";
        }

        // GET: Equipos
        public IActionResult Index()
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("Login", "Auth");
            }

            return View(_context.Equipos.Include(e => e.Sala).ToList());
        }

        // GET: Equipos/Create
        public IActionResult Create()
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.Salas = new SelectList(_context.Salas.ToList(), "Id", "Numero");
            return View();
        }

        // POST: Equipos/Create (Individual)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Nombre,Estado,Ubicacion,SalaId,Disponible")] Equipo equipo)
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (ModelState.IsValid)
            {
                equipo.Id = Guid.NewGuid();
                if (string.IsNullOrEmpty(equipo.Estado))
                {
                    equipo.Estado = "Libre";
                }
                // Si no se proporciona nombre, generar ID automático
                if (string.IsNullOrWhiteSpace(equipo.Nombre))
                {
                    equipo.Nombre = $"EQU-{equipo.Id.ToString().Substring(0, 8).ToUpper()}";
                }
                _context.Add(equipo);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Salas = new SelectList(_context.Salas.ToList(), "Id", "Numero", equipo.SalaId);
            return View(equipo);
        }

        // POST: Equipos/CreateMasivo (Creación masiva)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateMasivo(Guid salaId, string ubicacion, int cantidad, string estado = "Libre")
        {
            if (!IsAdministrator())
            {
                return Json(new { success = false, message = "No autorizado" });
            }

            if (cantidad <= 0 || cantidad > 100)
            {
                return Json(new { success = false, message = "La cantidad debe estar entre 1 y 100" });
            }

            var sala = _context.Salas.FirstOrDefault(s => s.Id == salaId);
            if (sala == null)
            {
                return Json(new { success = false, message = "Sala no encontrada" });
            }

            try
            {
                var equipos = new List<Equipo>();
                for (int i = 0; i < cantidad; i++)
                {
                    var equipo = new Equipo
                    {
                        Id = Guid.NewGuid(),
                        SalaId = salaId,
                        Ubicacion = ubicacion ?? sala.Ubicacion,
                        Estado = estado ?? "Libre",
                        Disponible = estado == "Libre",
                        Nombre = $"EQU-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}"
                    };
                    equipos.Add(equipo);
                }

                _context.Equipos.AddRange(equipos);
                _context.SaveChanges();

                return Json(new { success = true, message = $"Se crearon {cantidad} equipos correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al crear equipos: " + ex.Message });
            }
        }

        // GET: Equipos/Edit/5
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

            var equipo = _context.Equipos.Find(id);
            if (equipo == null)
            {
                return NotFound();
            }

            ViewBag.Salas = new SelectList(_context.Salas.ToList(), "Id", "Numero", equipo.SalaId);
            return View(equipo);
        }

        // POST: Equipos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,Nombre,Estado,Ubicacion,SalaId,Disponible")] Equipo equipo)
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id != equipo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(equipo);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipoExists(equipo.Id))
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

            ViewBag.Salas = new SelectList(_context.Salas.ToList(), "Id", "Numero", equipo.SalaId);
            return View(equipo);
        }

        // GET: Equipos/Delete/5
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

            var equipo = _context.Equipos
                .Include(e => e.Sala)
                .FirstOrDefault(m => m.Id == id);

            if (equipo == null)
            {
                return NotFound();
            }

            return View(equipo);
        }

        // POST: Equipos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("Login", "Auth");
            }

            var equipo = _context.Equipos
                .Include(e => e.SolicitudesEquipo)
                .FirstOrDefault(e => e.Id == id);

            if (equipo != null)
            {
                // Verificar si tiene solicitudes asociadas
                if (equipo.SolicitudesEquipo != null && equipo.SolicitudesEquipo.Any())
                {
                    TempData["Error"] = "No se puede eliminar el equipo porque tiene solicitudes asociadas.";
                    return RedirectToAction(nameof(Delete), new { id });
                }

                _context.Equipos.Remove(equipo);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EquipoExists(Guid id)
        {
            return _context.Equipos.Any(e => e.Id == id);
        }
    }
}
