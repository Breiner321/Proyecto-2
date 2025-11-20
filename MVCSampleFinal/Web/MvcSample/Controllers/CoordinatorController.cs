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

            var solicitudesAsesoria = new List<SolicitudAsesoria>().AsQueryable();
            try
            {
                solicitudesAsesoria = _context.SolicitudesAsesoria
                    .Include(s => s.Usuario)
                    .AsQueryable();
            }
            catch (Exception)
            {
                // Si la tabla no existe, usar lista vacía
                solicitudesAsesoria = new List<SolicitudAsesoria>().AsQueryable();
            }

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

                try
                {
                    solicitudesAsesoria = solicitudesAsesoria.Where(s => 
                        s.Descripcion.Contains(buscar) || 
                        s.Solicitante.Contains(buscar) ||
                        s.Estado.Contains(buscar) ||
                        s.TipoAsesoria.Contains(buscar));
                }
                catch (Exception)
                {
                    // Si hay error, mantener lista vacía
                }
            }

            ViewBag.SolicitudesSala = solicitudes.ToList();
            ViewBag.SolicitudesEquipo = solicitudesEquipo.ToList();
            try
            {
                ViewBag.SolicitudesAsesoria = solicitudesAsesoria.ToList();
            }
            catch (Exception)
            {
                ViewBag.SolicitudesAsesoria = new List<SolicitudAsesoria>();
            }

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
                    (e.Nombre != null && e.Nombre.Contains(buscar)) || 
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

        // GET: Coordinator/ReportesAvanzados
        public IActionResult ReportesAvanzados(string tipo = "diario", DateTime? fechaInicio = null, Guid? salaId = null)
        {
            if (!IsCoordinator())
            {
                return RedirectToAction("Login", "Auth");
            }

            var fechaConsulta = fechaInicio ?? DateTime.Now;
            var salas = _context.Salas.OrderBy(s => s.Numero).ToList();

            var reporteData = new List<object>();

            // Determinar rango de fechas según el tipo
            DateTime inicioRango, finRango;
            if (tipo == "diario")
            {
                inicioRango = fechaConsulta.Date;
                finRango = inicioRango.AddDays(1).AddTicks(-1);
            }
            else if (tipo == "semanal")
            {
                inicioRango = fechaConsulta.Date.AddDays(-(int)fechaConsulta.DayOfWeek);
                finRango = inicioRango.AddDays(7).AddTicks(-1);
            }
            else // mensual
            {
                inicioRango = new DateTime(fechaConsulta.Year, fechaConsulta.Month, 1);
                finRango = inicioRango.AddMonths(1).AddTicks(-1);
            }

            // Filtrar salas si se especifica una
            var salasFiltradas = salaId.HasValue 
                ? salas.Where(s => s.Id == salaId.Value).ToList()
                : salas;

            foreach (var sala in salasFiltradas)
            {
                var reservasSala = _context.Solicitudes
                    .Where(s => s.SalaId == sala.Id
                        && (s.Estado == "Aprobada" || s.Estado == "Pendiente" || s.Estado == "Finalizada")
                        && s.Fecha >= inicioRango
                        && s.Fecha <= finRango)
                    .ToList();

                var reservasEquipos = _context.SolicitudesEquipo
                    .Include(se => se.Equipo)
                    .Where(se => se.Equipo.SalaId == sala.Id
                        && (se.Estado == "Aprobada" || se.Estado == "Pendiente" || se.Estado == "Finalizada")
                        && se.Fecha >= inicioRango
                        && se.Fecha <= finRango)
                    .ToList();

                reporteData.Add(new
                {
                    Sala = sala,
                    Tipo = tipo,
                    FechaInicio = inicioRango,
                    FechaFin = finRango,
                    ReservasSala = reservasSala,
                    ReservasEquipos = reservasEquipos,
                    TotalReservasSala = reservasSala.Count,
                    TotalReservasEquipos = reservasEquipos.Count,
                    ReservasAprobadas = reservasSala.Count(s => s.Estado == "Aprobada") + reservasEquipos.Count(s => s.Estado == "Aprobada"),
                    ReservasPendientes = reservasSala.Count(s => s.Estado == "Pendiente") + reservasEquipos.Count(s => s.Estado == "Pendiente"),
                    ReservasFinalizadas = reservasSala.Count(s => s.Estado == "Finalizada") + reservasEquipos.Count(s => s.Estado == "Finalizada")
                });
            }

            ViewBag.Tipo = tipo;
            ViewBag.FechaInicio = fechaConsulta;
            ViewBag.SalaId = salaId;
            ViewBag.Salas = salas;
            ViewBag.ReporteData = reporteData;

            return View();
        }

        // GET: Coordinator/OcupacionSalas
        public IActionResult OcupacionSalas(string tipo = "diario", DateTime? fecha = null)
        {
            if (!IsCoordinator())
            {
                return RedirectToAction("Login", "Auth");
            }

            // Si no se proporciona fecha, usar la fecha actual
            var fechaConsulta = fecha ?? DateTime.Now;
            var inicioDia = fechaConsulta.Date;
            var finDia = inicioDia.AddDays(1).AddTicks(-1);

            var salas = _context.Salas
                .Include(s => s.Equipos)
                .OrderBy(s => s.Numero)
                .ToList();

            var reporteSalas = new List<object>();

            foreach (var sala in salas)
            {
                if (tipo == "diario")
                {
                    // Reporte diario
                    var reservasSala = _context.Solicitudes
                        .Where(s => s.SalaId == sala.Id 
                            && (s.Estado == "Aprobada" || s.Estado == "Pendiente")
                            && s.FechaHoraInicio.HasValue 
                            && s.FechaHoraFin.HasValue
                            && ((s.FechaHoraInicio.Value >= inicioDia && s.FechaHoraInicio.Value < finDia) ||
                                (s.FechaHoraFin.Value >= inicioDia && s.FechaHoraFin.Value < finDia) ||
                                (s.FechaHoraInicio.Value <= inicioDia && s.FechaHoraFin.Value >= finDia)))
                        .ToList();

                    var reservasEquipos = _context.SolicitudesEquipo
                        .Include(se => se.Equipo)
                        .Where(se => se.Equipo.SalaId == sala.Id
                            && (se.Estado == "Aprobada" || se.Estado == "Pendiente")
                            && se.FechaHoraInicio.HasValue 
                            && se.FechaHoraFin.HasValue
                            && ((se.FechaHoraInicio.Value >= inicioDia && se.FechaHoraInicio.Value < finDia) ||
                                (se.FechaHoraFin.Value >= inicioDia && se.FechaHoraFin.Value < finDia) ||
                                (se.FechaHoraInicio.Value <= inicioDia && se.FechaHoraFin.Value >= finDia)))
                        .ToList();

                    var horasOcupadas = new List<object>();
                    for (int hora = 0; hora < 24; hora++)
                    {
                        var horaInicio = inicioDia.AddHours(hora);
                        var horaFin = horaInicio.AddHours(1);

                        var ocupadaPorSala = reservasSala.Any(r => 
                            r.FechaHoraInicio.Value < horaFin && r.FechaHoraFin.Value > horaInicio);

                        var ocupadaPorEquipos = reservasEquipos.Any(r => 
                            r.FechaHoraInicio.Value < horaFin && r.FechaHoraFin.Value > horaInicio);

                        horasOcupadas.Add(new
                        {
                            Hora = hora,
                            HoraTexto = $"{hora:00}:00 - {(hora + 1):00}:00",
                            Ocupada = ocupadaPorSala || ocupadaPorEquipos,
                            Tipo = ocupadaPorSala ? "Sala" : (ocupadaPorEquipos ? "Equipos" : "Libre")
                        });
                    }

                    reporteSalas.Add(new
                    {
                        Sala = sala,
                        Fecha = fechaConsulta,
                        HorasOcupadas = horasOcupadas,
                        TotalHorasOcupadas = horasOcupadas.Count(h => ((dynamic)h).Ocupada),
                        ReservasSala = reservasSala,
                        ReservasEquipos = reservasEquipos
                    });
                }
                else
                {
                    // Reporte semanal
                    var inicioSemana = fechaConsulta.Date.AddDays(-(int)fechaConsulta.DayOfWeek);
                    var finSemana = inicioSemana.AddDays(7);

                    var reservasSala = _context.Solicitudes
                        .Where(s => s.SalaId == sala.Id 
                            && (s.Estado == "Aprobada" || s.Estado == "Pendiente")
                            && s.FechaHoraInicio.HasValue 
                            && s.FechaHoraFin.HasValue
                            && s.FechaHoraInicio.Value >= inicioSemana 
                            && s.FechaHoraInicio.Value < finSemana)
                        .ToList();

                    var reservasEquipos = _context.SolicitudesEquipo
                        .Include(se => se.Equipo)
                        .Where(se => se.Equipo.SalaId == sala.Id
                            && (se.Estado == "Aprobada" || se.Estado == "Pendiente")
                            && se.FechaHoraInicio.HasValue 
                            && se.FechaHoraFin.HasValue
                            && se.FechaHoraInicio.Value >= inicioSemana 
                            && se.FechaHoraInicio.Value < finSemana)
                        .ToList();

                    var diasSemana = new List<object>();
                    for (int dia = 0; dia < 7; dia++)
                    {
                        var fechaDia = inicioSemana.AddDays(dia);
                        var reservasDiaSala = reservasSala.Where(r => 
                            r.FechaHoraInicio.Value.Date == fechaDia.Date).Count();
                        var reservasDiaEquipos = reservasEquipos.Where(r => 
                            r.FechaHoraInicio.Value.Date == fechaDia.Date).Count();

                        diasSemana.Add(new
                        {
                            Fecha = fechaDia,
                            DiaNombre = fechaDia.ToString("dddd", new System.Globalization.CultureInfo("es-ES")),
                            ReservasSala = reservasDiaSala,
                            ReservasEquipos = reservasDiaEquipos,
                            TotalReservas = reservasDiaSala + reservasDiaEquipos
                        });
                    }

                    reporteSalas.Add(new
                    {
                        Sala = sala,
                        InicioSemana = inicioSemana,
                        FinSemana = finSemana,
                        DiasSemana = diasSemana,
                        TotalReservasSala = reservasSala.Count,
                        TotalReservasEquipos = reservasEquipos.Count
                    });
                }
            }

            ViewBag.Tipo = tipo;
            ViewBag.Fecha = fechaConsulta;
            ViewBag.ReporteSalas = reporteSalas;

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

        // POST: Coordinator/AprobarSolicitudSala
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AprobarSolicitudSala(Guid solicitudId)
        {
            if (!IsCoordinator())
            {
                return Json(new { success = false, message = "No autorizado" });
            }

            var solicitud = _context.Solicitudes
                .Include(s => s.Usuario)
                .FirstOrDefault(s => s.Id == solicitudId);

            if (solicitud == null)
            {
                return Json(new { success = false, message = "Solicitud no encontrada" });
            }

            solicitud.Estado = "Aprobada";
            
            // Actualizar estado de la sala si es necesario
            var sala = _context.Salas.FirstOrDefault(s => s.Id == solicitud.SalaId);
            if (sala != null && sala.Estado != "Ocupada")
            {
                sala.Estado = "Ocupada";
            }

            try
            {
                _context.SaveChanges();
                return Json(new { success = true, message = "Solicitud aprobada correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al aprobar solicitud: " + ex.Message });
            }
        }

        // POST: Coordinator/RechazarSolicitudSala
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RechazarSolicitudSala(Guid solicitudId)
        {
            if (!IsCoordinator())
            {
                return Json(new { success = false, message = "No autorizado" });
            }

            var solicitud = _context.Solicitudes.FirstOrDefault(s => s.Id == solicitudId);

            if (solicitud == null)
            {
                return Json(new { success = false, message = "Solicitud no encontrada" });
            }

            solicitud.Estado = "Rechazada";

            try
            {
                _context.SaveChanges();
                return Json(new { success = true, message = "Solicitud rechazada correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al rechazar solicitud: " + ex.Message });
            }
        }

        // POST: Coordinator/AprobarSolicitudEquipo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AprobarSolicitudEquipo(Guid solicitudId)
        {
            if (!IsCoordinator())
            {
                return Json(new { success = false, message = "No autorizado" });
            }

            var solicitud = _context.SolicitudesEquipo
                .Include(s => s.Usuario)
                .Include(s => s.Equipo)
                .FirstOrDefault(s => s.Id == solicitudId);

            if (solicitud == null)
            {
                return Json(new { success = false, message = "Solicitud no encontrada" });
            }

            solicitud.Estado = "Aprobada";
            
            // Actualizar estado del equipo
            if (solicitud.Equipo != null)
            {
                solicitud.Equipo.Estado = "Asignado";
                solicitud.Equipo.Disponible = false;
            }

            try
            {
                _context.SaveChanges();
                return Json(new { success = true, message = "Solicitud aprobada correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al aprobar solicitud: " + ex.Message });
            }
        }

        // POST: Coordinator/RechazarSolicitudEquipo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RechazarSolicitudEquipo(Guid solicitudId)
        {
            if (!IsCoordinator())
            {
                return Json(new { success = false, message = "No autorizado" });
            }

            var solicitud = _context.SolicitudesEquipo.FirstOrDefault(s => s.Id == solicitudId);

            if (solicitud == null)
            {
                return Json(new { success = false, message = "Solicitud no encontrada" });
            }

            solicitud.Estado = "Rechazada";

            try
            {
                _context.SaveChanges();
                return Json(new { success = true, message = "Solicitud rechazada correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al rechazar solicitud: " + ex.Message });
            }
        }

        // POST: Coordinator/AprobarSolicitudAsesoria
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AprobarSolicitudAsesoria(Guid solicitudId)
        {
            if (!IsCoordinator())
            {
                return Json(new { success = false, message = "No autorizado" });
            }

            var solicitud = _context.SolicitudesAsesoria
                .Include(s => s.Usuario)
                .FirstOrDefault(s => s.Id == solicitudId);

            if (solicitud == null)
            {
                return Json(new { success = false, message = "Solicitud no encontrada" });
            }

            solicitud.Estado = "Aprobada";

            try
            {
                _context.SaveChanges();
                return Json(new { success = true, message = "Solicitud de asesoría aprobada correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al aprobar solicitud: " + ex.Message });
            }
        }

        // POST: Coordinator/RechazarSolicitudAsesoria
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RechazarSolicitudAsesoria(Guid solicitudId, string observaciones = "")
        {
            if (!IsCoordinator())
            {
                return Json(new { success = false, message = "No autorizado" });
            }

            var solicitud = _context.SolicitudesAsesoria
                .Include(s => s.Usuario)
                .FirstOrDefault(s => s.Id == solicitudId);

            if (solicitud == null)
            {
                return Json(new { success = false, message = "Solicitud no encontrada" });
            }

            solicitud.Estado = "Rechazada";
            if (!string.IsNullOrWhiteSpace(observaciones))
            {
                solicitud.Observaciones = observaciones;
            }

            try
            {
                _context.SaveChanges();
                return Json(new { success = true, message = "Solicitud de asesoría rechazada correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al rechazar solicitud: " + ex.Message });
            }
        }

        // GET: Coordinator/ReportesDano
        public IActionResult ReportesDano(string buscar, string estadoFiltro = "")
        {
            if (!IsCoordinator())
            {
                return RedirectToAction("Login", "Auth");
            }

            var reportes = _context.ReportesDano
                .Include(r => r.Usuario)
                .Include(r => r.Equipo)
                    .ThenInclude(e => e.Sala)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                reportes = reportes.Where(r => 
                    r.Descripcion.Contains(buscar) || 
                    r.Tipo.Contains(buscar) ||
                    (r.Usuario != null && r.Usuario.Nombre.Contains(buscar)));
            }

            if (!string.IsNullOrWhiteSpace(estadoFiltro))
            {
                reportes = reportes.Where(r => r.Estado == estadoFiltro);
            }

            var salas = _context.Salas.ToList();
            
            ViewBag.ReportesDano = reportes.OrderByDescending(r => r.Fecha).ToList();
            ViewBag.Salas = salas;
            ViewBag.Buscar = buscar;
            ViewBag.EstadoFiltro = estadoFiltro;

            return View();
        }

        // POST: Coordinator/ActualizarEstadoReporteDano
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActualizarEstadoReporteDano(Guid reporteId, string nuevoEstado, string observaciones = "")
        {
            if (!IsCoordinator())
            {
                return Json(new { success = false, message = "No autorizado" });
            }

            var reporte = _context.ReportesDano
                .Include(r => r.Equipo)
                .FirstOrDefault(r => r.Id == reporteId);

            if (reporte == null)
            {
                return Json(new { success = false, message = "Reporte no encontrado" });
            }

            var estadoAnterior = reporte.Estado;
            reporte.Estado = nuevoEstado;
            
            if (!string.IsNullOrWhiteSpace(observaciones))
            {
                reporte.Observaciones = observaciones;
            }

            try
            {
                // Si el estado cambia a "Resuelto" y es un equipo, marcarlo como disponible
                if (nuevoEstado == "Resuelto" && reporte.Tipo == "Equipo" && reporte.EquipoId.HasValue)
                {
                    var equipo = _context.Equipos.FirstOrDefault(e => e.Id == reporte.EquipoId.Value);
                    if (equipo != null)
                    {
                        equipo.Estado = "Libre";
                        equipo.Disponible = true;
                    }
                }

                // Si el estado cambia a "Resuelto" y es una sala, marcarla como disponible
                if (nuevoEstado == "Resuelto" && reporte.Tipo == "Sala" && reporte.SalaId.HasValue)
                {
                    var sala = _context.Salas.FirstOrDefault(s => s.Id == reporte.SalaId.Value);
                    if (sala != null)
                    {
                        sala.Estado = "Disponible";
                    }
                }

                _context.SaveChanges();
                return Json(new { success = true, message = "Estado del reporte actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al actualizar el reporte: " + ex.Message });
            }
        }
    }
}

