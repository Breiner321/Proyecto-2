using Microsoft.AspNetCore.Mvc;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MvcSample.Controllers
{
    public class CoordinatorController : Controller
    {
        private readonly AppDbContext _context;

        public CoordinatorController(AppDbContext context)
        {
            _context = context;
        }

        // Verificar que el usuario sea coordinador
        private bool IsCoordinator()
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");
            return rol == "Coordinador";
        }

        // GET: Coordinator/EstadoOcupacion
        public IActionResult EstadoOcupacion()
        {
            if (!IsCoordinator())
            {
                return RedirectToAction("Login", "Auth");
            }

            var salas = _context.Salas
                .Include(s => s.Equipos)
                .ToList();

            return View(salas);
        }

        // GET: Coordinator/Solicitudes
        public IActionResult Solicitudes(string buscar)
        {
            if (!IsCoordinator())
            {
                return RedirectToAction("Login", "Auth");
            }

            var solicitudes = _context.Solicitudes
                .Include(s => s.Usuario)
                .AsQueryable();

            var solicitudesEquipo = _context.SolicitudesEquipo
                .Include(s => s.Usuario)
                .Include(s => s.Equipo)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                solicitudes = solicitudes.Where(s => 
                    s.Descripcion.Contains(buscar) || 
                    s.Solicitante.Contains(buscar) ||
                    s.Estado.Contains(buscar));

                solicitudesEquipo = solicitudesEquipo.Where(s => 
                    s.Descripcion.Contains(buscar) || 
                    s.Solicitante.Contains(buscar) ||
                    s.Estado.Contains(buscar));
            }

            ViewBag.SolicitudesSala = solicitudes.ToList();
            ViewBag.SolicitudesEquipo = solicitudesEquipo.ToList();

            return View();
        }

        // GET: Coordinator/Equipos
        public IActionResult Equipos(string sala = "Sala A", string buscar = "")
        {
            if (!IsCoordinator())
            {
                return RedirectToAction("Login", "Auth");
            }

            // Obtener todas las salas para los tabs
            var todasLasSalas = _context.Salas.ToList();
            
            // Si no hay salas, crear algunas de ejemplo
            if (!todasLasSalas.Any())
            {
                ViewBag.Salas = new List<Sala>
                {
                    new Sala { Id = Guid.NewGuid(), Numero = "A", Ubicacion = "Edificio Principal" },
                    new Sala { Id = Guid.NewGuid(), Numero = "B", Ubicacion = "Edificio Principal" },
                    new Sala { Id = Guid.NewGuid(), Numero = "C", Ubicacion = "Edificio Principal" }
                };
            }
            else
            {
                ViewBag.Salas = todasLasSalas;
            }

            // Obtener equipos de la sala seleccionada
            var salaSeleccionada = todasLasSalas.FirstOrDefault(s => s.Numero == sala.Replace("Sala ", ""));
            
            var equipos = _context.Equipos
                .Include(e => e.Sala)
                .Include(e => e.SolicitudesEquipo)
                    .ThenInclude(se => se.Usuario)
                .AsQueryable();

            if (salaSeleccionada != null)
            {
                equipos = equipos.Where(e => e.SalaId == salaSeleccionada.Id);
            }

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                equipos = equipos.Where(e => 
                    e.Nombre.Contains(buscar) || 
                    e.Estado.Contains(buscar) ||
                    e.Ubicacion.Contains(buscar));
            }

            ViewBag.SalaSeleccionada = sala;
            ViewBag.Buscar = buscar;

            return View(equipos.ToList());
        }

        // GET: Coordinator/Reportes
        public IActionResult Reportes()
        {
            if (!IsCoordinator())
            {
                return RedirectToAction("Login", "Auth");
            }

            var totalSalas = _context.Salas.Count();
            var totalEquipos = _context.Equipos.Count();
            var equiposDisponibles = _context.Equipos.Count(e => e.Disponible);
            var equiposOcupados = totalEquipos - equiposDisponibles;
            var totalSolicitudes = _context.Solicitudes.Count() + _context.SolicitudesEquipo.Count();
            var solicitudesPendientes = _context.Solicitudes.Count(s => s.Estado == "Pendiente") + 
                                       _context.SolicitudesEquipo.Count(s => s.Estado == "Pendiente");

            ViewBag.TotalSalas = totalSalas;
            ViewBag.TotalEquipos = totalEquipos;
            ViewBag.EquiposDisponibles = equiposDisponibles;
            ViewBag.EquiposOcupados = equiposOcupados;
            ViewBag.TotalSolicitudes = totalSolicitudes;
            ViewBag.SolicitudesPendientes = solicitudesPendientes;

            return View();
        }

        // GET: Coordinator/Configuracion
        public IActionResult Configuracion()
        {
            if (!IsCoordinator())
            {
                return RedirectToAction("Login", "Auth");
            }

            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (Guid.TryParse(usuarioId, out Guid id))
            {
                var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
                return View(usuario);
            }

            return View();
        }

        // POST: Coordinator/ActualizarEstadoEquipo
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult ActualizarEstadoEquipo(Guid equipoId, string nuevoEstado)
        {
            if (!IsCoordinator())
            {
                return Json(new { success = false, message = "No autorizado" });
            }

            var equipo = _context.Equipos.FirstOrDefault(e => e.Id == equipoId);
            if (equipo == null)
            {
                return Json(new { success = false, message = "Equipo no encontrado" });
            }

            equipo.Estado = nuevoEstado;
            equipo.Disponible = nuevoEstado == "Libre";

            try
            {
                _context.SaveChanges();
                return Json(new { success = true, message = "Estado actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al actualizar: " + ex.Message });
            }
        }
    }
}

