using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrackingPedidos.Models;
using TrackingPedidos.Services;
using Vereyon.Web;

namespace TrackingPedidos.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class SeguimientosController : Controller
    {
        private readonly TrackingContext _context;
        private readonly ApiService _api;
        public IFlashMessage _flashMessage { get; private set; }

        public SeguimientosController(TrackingContext context, IFlashMessage flashMessage)
        {
            _context = context;
            _api = new ApiService();
            _flashMessage = flashMessage;
        }

        // GET: Seguimientos
        public async Task<IActionResult> Index()
        {
            var seguimiento = await _context.Pedidos.OrderByDescending(i => i.PedFechaDespachado).AsNoTracking().ToListAsync();
            return View(seguimiento);
        }

        // GET: Seguimientos/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // POST: Seguimientos/PackOff/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PackOff(int id)
        {
            try
            {
                var seguimiento = await _context.Pedidos.FindAsync(id);
                if (seguimiento == null)
                {
                    return NotFound();
                }

                seguimiento.PedFase = 'I';
                seguimiento.PedFechaDespachado = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);

                _flashMessage.Confirmation($"Pedido con Factura Nro {seguimiento.InvoiceNumber} despachado.");

                await _context.SaveChangesAsync();
            }
            catch
            {
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Seguimientos/OnWay/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnWay(int id)
        {
            try
            {
                var seguimiento = await _context.Pedidos.FindAsync(id);
                if (seguimiento == null)
                {
                    return NotFound();
                }

                seguimiento.PedFase = 'C';
                seguimiento.PedFechaCamino = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);

                _flashMessage.Confirmation($"Pedido con Factura Nro {seguimiento.InvoiceNumber} en camino.");

                await _context.SaveChangesAsync();
            }
            catch
            {
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Seguimiento/End/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> End(int id)
        {
            try
            {
                var seguimiento = await _context.Pedidos.FindAsync(id);
                if (seguimiento == null)
                {
                    return NotFound();
                }

                seguimiento.PedFase = 'F';
                seguimiento.PedFechaFin = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);

                _flashMessage.Confirmation($"Pedido con Factura Nro {seguimiento.InvoiceNumber} finalizado.");

                await _context.SaveChangesAsync();
            }
            catch
            {
            }
            return RedirectToAction(nameof(Index));
        }
    }
}