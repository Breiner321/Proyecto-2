using Microsoft.AspNetCore.Mvc;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MvcSample.Controllers
{
    public class StudentController : Controller
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        // Verificar que el usuario sea estudiante
        private bool IsStudent()
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");
            return rol == "Estudiante" || rol == "Usuario";
        }

        private Guid? GetUserId()
        {
            var userIdString = HttpContext.Session.GetString("UsuarioId");
            if (Guid.TryParse(userIdString, out Guid userId))
            {
                return userId;
            }
            return null;
        }

        // GET: Student/ReservarSala
        public IActionResult ReservarSala(string buscar, string fecha, string hora, string salaSeleccionada = "A")
        {
            if (!IsStudent())
            {
                return RedirectToAction("Login", "Auth");
            }

            // Obtener la sala seleccionada
            var sala = _context.Salas
                .Include(s => s.Equipos)
                .FirstOrDefault(s => s.Numero == salaSeleccionada);

            // Obtener equipos de la sala
            var equipos = new List<Equipo>();
            if (sala != null)
            {
                equipos = sala.Equipos?.ToList() ?? new List<Equipo>();
            }

            // Obtener salas libres (disponibles)
            var salasLibres = _context.Salas
                .Where(s => s.Estado == "Disponible" || string.IsNullOrEmpty(s.Estado))
                .ToList();

            ViewBag.SalaSeleccionada = salaSeleccionada;
            ViewBag.Equipos = equipos;
            ViewBag.SalasLibres = salasLibres;
            ViewBag.Buscar = buscar;
            ViewBag.Fecha = fecha;
            ViewBag.Hora = hora;

            return View();
        }

        // POST: Student/ReservarEquipo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReservarEquipo(Guid equipoId, string descripcion, DateTime? fechaReserva)
        {
            if (!IsStudent())
            {
                return Json(new { success = false, message = "No autorizado" });
            }

            var userId = GetUserId();
            if (!userId.HasValue)
            {
                return Json(new { success = false, message = "Usuario no identificado" });
            }

            var equipo = _context.Equipos.FirstOrDefault(e => e.Id == equipoId);
            if (equipo == null)
            {
                return Json(new { success = false, message = "Equipo no encontrado" });
            }

            if (!equipo.Disponible || equipo.Estado == "Bloqueado")
            {
                return Json(new { success = false, message = "El equipo no está disponible" });
            }

            var solicitud = new SolicitudEquipo
            {
                Id = Guid.NewGuid(),
                EquipoId = equipoId,
                UsuarioId = userId.Value,
                Descripcion = descripcion ?? "Reserva de equipo",
                Fecha = fechaReserva ?? DateTime.Now,
                Estado = "Pendiente",
                Solicitante = HttpContext.Session.GetString("UsuarioNombre") ?? "Estudiante"
            };

            _context.SolicitudesEquipo.Add(solicitud);
            _context.SaveChanges();

            return Json(new { success = true, message = "Solicitud de reserva enviada correctamente" });
        }

        // POST: Student/ReservarSala
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReservarSala(Guid salaId, string descripcion, DateTime? fechaReserva)
        {
            if (!IsStudent())
            {
                return Json(new { success = false, message = "No autorizado" });
            }

            var userId = GetUserId();
            if (!userId.HasValue)
            {
                return Json(new { success = false, message = "Usuario no identificado" });
            }

            var sala = _context.Salas.FirstOrDefault(s => s.Id == salaId);
            if (sala == null)
            {
                return Json(new { success = false, message = "Sala no encontrada" });
            }

            if (sala.Estado == "Ocupada" || sala.Estado == "Mantenimiento")
            {
                return Json(new { success = false, message = "La sala no está disponible" });
            }

            var solicitud = new Solicitud
            {
                Id = Guid.NewGuid(),
                SalaId = salaId,
                UsuarioId = userId.Value,
                Descripcion = descripcion ?? "Reserva de sala",
                Fecha = fechaReserva ?? DateTime.Now,
                Estado = "Pendiente",
                Solicitante = HttpContext.Session.GetString("UsuarioNombre") ?? "Estudiante"
            };

            _context.Solicitudes.Add(solicitud);
            _context.SaveChanges();

            return Json(new { success = true, message = "Solicitud de reserva enviada correctamente" });
        }

        // GET: Student/MisReservas
        public IActionResult MisReservas()
        {
            if (!IsStudent())
            {
                return RedirectToAction("Login", "Auth");
            }

            var userId = GetUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var solicitudesSala = _context.Solicitudes
                .Include(s => s.Usuario)
                .Where(s => s.UsuarioId == userId.Value)
                .OrderByDescending(s => s.Fecha)
                .ToList();

            var solicitudesEquipo = _context.SolicitudesEquipo
                .Include(s => s.Usuario)
                .Include(s => s.Equipo)
                .Where(s => s.UsuarioId == userId.Value)
                .OrderByDescending(s => s.Fecha)
                .ToList();

            ViewBag.SolicitudesSala = solicitudesSala;
            ViewBag.SolicitudesEquipo = solicitudesEquipo;

            return View();
        }

        // GET: Student/Reportes
        public IActionResult Reportes()
        {
            if (!IsStudent())
            {
                return RedirectToAction("Login", "Auth");
            }

            var userId = GetUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var totalReservas = _context.Solicitudes.Count(s => s.UsuarioId == userId.Value) +
                               _context.SolicitudesEquipo.Count(s => s.UsuarioId == userId.Value);

            var reservasAprobadas = _context.Solicitudes.Count(s => s.UsuarioId == userId.Value && s.Estado == "Aprobada") +
                                   _context.SolicitudesEquipo.Count(s => s.UsuarioId == userId.Value && s.Estado == "Aprobada");

            var reservasPendientes = _context.Solicitudes.Count(s => s.UsuarioId == userId.Value && s.Estado == "Pendiente") +
                                    _context.SolicitudesEquipo.Count(s => s.UsuarioId == userId.Value && s.Estado == "Pendiente");

            ViewBag.TotalReservas = totalReservas;
            ViewBag.ReservasAprobadas = reservasAprobadas;
            ViewBag.ReservasPendientes = reservasPendientes;

            return View();
        }
    }
}

