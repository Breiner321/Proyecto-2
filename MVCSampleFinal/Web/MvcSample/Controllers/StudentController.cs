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

        // GET: Student/ReservarEquipo
        public IActionResult ReservarEquipo(string buscar = "")
        {
            if (!IsStudent())
            {
                return RedirectToAction("Login", "Auth");
            }

            var equipos = _context.Equipos
                .Include(e => e.Sala)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                equipos = equipos.Where(e => 
                    (e.Nombre != null && e.Nombre.Contains(buscar)) || 
                    e.Ubicacion.Contains(buscar) ||
                    (e.Sala != null && e.Sala.Numero.Contains(buscar)));
            }

            ViewBag.Equipos = equipos.ToList();
            ViewBag.Buscar = buscar;

            return View();
        }

        // POST: Student/ReservarEquipo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReservarEquipo(Guid equipoId, string descripcion, DateTime? fechaHoraInicio, DateTime? fechaHoraFin)
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

            // Validar fechas
            var inicio = fechaHoraInicio ?? DateTime.Now;
            var fin = fechaHoraFin ?? inicio.AddHours(6);

            if (fin <= inicio)
            {
                return Json(new { success = false, message = "La fecha/hora de fin debe ser posterior a la de inicio" });
            }

            var duracion = fin - inicio;
            if (duracion.TotalHours > 6)
            {
                return Json(new { success = false, message = "El tiempo máximo de reserva es de 6 horas" });
            }

            var solicitud = new SolicitudEquipo
            {
                Id = Guid.NewGuid(),
                EquipoId = equipoId,
                UsuarioId = userId.Value,
                Descripcion = descripcion ?? "Reserva de equipo",
                Fecha = DateTime.Now,
                FechaHoraInicio = inicio,
                FechaHoraFin = fin,
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
        public IActionResult ReservarSala(Guid salaId, string descripcion, DateTime? fechaHoraInicio, DateTime? fechaHoraFin)
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

            // Validar fechas
            var inicio = fechaHoraInicio ?? DateTime.Now;
            var fin = fechaHoraFin ?? inicio.AddHours(6);

            if (fin <= inicio)
            {
                return Json(new { success = false, message = "La fecha/hora de fin debe ser posterior a la de inicio" });
            }

            var duracion = fin - inicio;
            if (duracion.TotalHours > 6)
            {
                return Json(new { success = false, message = "El tiempo máximo de reserva es de 6 horas" });
            }

            var solicitud = new Solicitud
            {
                Id = Guid.NewGuid(),
                SalaId = salaId,
                UsuarioId = userId.Value,
                Descripcion = descripcion ?? "Reserva de sala",
                Fecha = DateTime.Now,
                FechaHoraInicio = inicio,
                FechaHoraFin = fin,
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

            // Cargar solo los IDs primero para evitar problemas con columnas faltantes
            var solicitudesSalaIds = _context.Solicitudes
                .Where(s => s.UsuarioId == userId.Value)
                .Select(s => s.Id)
                .ToList();

            var solicitudesSala = _context.Solicitudes
                .Include(s => s.Usuario)
                .Where(s => solicitudesSalaIds.Contains(s.Id))
                .OrderByDescending(s => s.Fecha)
                .ToList();

            var solicitudesEquipoIds = _context.SolicitudesEquipo
                .Where(s => s.UsuarioId == userId.Value)
                .Select(s => s.Id)
                .ToList();

            var solicitudesEquipo = _context.SolicitudesEquipo
                .Include(s => s.Usuario)
                .Include(s => s.Equipo)
                .Where(s => solicitudesEquipoIds.Contains(s.Id))
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

        // POST: Student/LiberarEquipo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LiberarEquipo(Guid solicitudId)
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

            var solicitud = _context.SolicitudesEquipo
                .Include(s => s.Equipo)
                .FirstOrDefault(s => s.Id == solicitudId && s.UsuarioId == userId.Value);

            if (solicitud == null)
            {
                return Json(new { success = false, message = "Solicitud no encontrada o no autorizada" });
            }

            if (solicitud.Estado != "Aprobada")
            {
                return Json(new { success = false, message = "Solo se pueden liberar equipos de solicitudes aprobadas" });
            }

            // Actualizar estado del equipo
            if (solicitud.Equipo != null)
            {
                solicitud.Equipo.Estado = "Libre";
                solicitud.Equipo.Disponible = true;
            }

            // Actualizar estado de la solicitud
            solicitud.Estado = "Finalizada";
            solicitud.FechaHoraFin = DateTime.Now;

            try
            {
                _context.SaveChanges();
                return Json(new { success = true, message = "Equipo liberado correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al liberar el equipo: " + ex.Message });
            }
        }
    }
}

