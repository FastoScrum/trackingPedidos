using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrackingPedidos.Models;

namespace TrackingPedidos.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ReportesController : Controller
    {
        private readonly TrackingContext _context;

        public ReportesController(TrackingContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> General()
        {
            var seguimientos = await _context.Pedidos.OrderByDescending(i => i.PedFechaEnvio).AsNoTracking().ToListAsync();
            return View(seguimientos);
            //return new ViewAsPdf(seguimientos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PorFecha(DateTime fecha)
        {
            return RedirectToAction(nameof(PorFecha), new { year = fecha.Year, mes = fecha.Month, dia = fecha.Day });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PorEstado(string estado)
        {
            return RedirectToAction(nameof(Pedidos), new { estado });
        }

        [HttpGet("[controller]/[action]/{year:int}/{mes:int}/{dia:int}")]
        public async Task<IActionResult> PorFecha(int year, int mes, int dia)
        {
            try
            {
                var seguimientos = await _context.Pedidos
                    .Where(i => i.PedFechaEnvio.Year == year)
                    .Where(i => i.PedFechaEnvio.Month == mes)
                    .Where(i => i.PedFechaEnvio.Day == dia)
                    .OrderByDescending(i => i.PedFechaDespachado)
                    .AsNoTracking().ToListAsync();

                ViewData["Fecha"] = new DateTime(year, mes, dia).ToShortDateString();
                //return new ViewAsPdf(seguimientos, ViewData);
                return View(seguimientos);
            }
            catch
            {
            }
            return NotFound();
        }

        public async Task<IActionResult> Pedidos([FromQuery(Name = "estado")] string estado)
        {
            estado = estado.ToUpper();

            if (!estado.Equals("E") && !estado.Equals("C") && !estado.Equals("F"))
            {
                return NotFound();
            }

            ViewData["Estado"] = estado;

            if (estado.Equals("E"))
            {
                ViewData["Title"] = "Reporte de pedidos entregados";
                var seguimientos = await _context.Pedidos
                                .Where(i => i.PedFase.ToString() == "E")
                                    .OrderByDescending(i => i.PedFechaEntrega)
                                    .AsNoTracking().ToListAsync();

                //return new ViewAsPdf(seguimientos, ViewData);
                return View(seguimientos);
            }
            else if (estado.Equals("C"))
            {
                ViewData["Title"] = "Reporte de pedidos en camino";
                var seguimientos = await _context.Pedidos
                                .Where(i => i.PedFase.ToString() == "C")
                                    .OrderByDescending(i => i.PedFechaCamino)
                                    .AsNoTracking().ToListAsync();

                return View(seguimientos);
                //return new ViewAsPdf(seguimientos, ViewData);
            }
            else
            {
                ViewData["Title"] = "Reporte de pedidos en distribuidora";
                var seguimientos = await _context.Pedidos
                                .Where(i => i.PedFase.ToString() == "F")
                                    .OrderByDescending(i => i.PedFechaFin)
                                    .AsNoTracking().ToListAsync();

                return View(seguimientos);
                //return new ViewAsPdf(seguimientos, ViewData);
            }
        }
    }
}