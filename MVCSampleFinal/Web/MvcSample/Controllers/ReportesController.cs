using Microsoft.AspNetCore.Mvc;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Domain;
using System.Linq;

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

        // GET: Reportes/ReportesAvanzados
        public IActionResult ReportesAvanzados(string tipo = "diario", DateTime? fechaInicio = null, Guid? salaId = null)
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");
            if (rol != "Administrador")
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
    }
}

