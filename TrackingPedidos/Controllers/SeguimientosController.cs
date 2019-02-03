using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrackingPedidos.Models;
using TrackingPedidos.Services;

namespace TrackingPedidos.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class SeguimientosController : Controller
    {
        private readonly TrackingContext _context;
        private readonly ApiService _api;

        public SeguimientosController(TrackingContext context)
        {
            _context = context;
            _api = new ApiService();
        }

        // GET: Seguimiento
        public async Task<IActionResult> Index()
        {
            var seguimiento = await _context.Pedidos.OrderByDescending(i => i.PedFechaDespachado).AsNoTracking().ToListAsync();
            return View(seguimiento);
        }

        // GET: Seguimiento/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // POST: Seguimiento/PackOff/5
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

                await _context.SaveChangesAsync();
            }
            catch
            {
            }
            return RedirectToAction(nameof(Index));
        }
    }
}