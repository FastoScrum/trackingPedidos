using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrackingPedidos.Models;
using TrackingPedidos.Services;
using TrackingPedidos.ViewModels;
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
                _flashMessage.Danger($"Error al despachar el pedido.");
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
                _flashMessage.Danger($"Error al poner en camino el pedido.");
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
                _flashMessage.Danger($"Error al finalizar el pedido.");
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Seguimiento/Delivery/584848445484
        public IActionResult Delivery(string id)
        {
            return View();
        }

        // POST: Seguimiento/Delivery/584848445484
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delivery(string id, EntregaVM _entrega)
        {
            if (!ModelState.IsValid)
            {
                return View(_entrega);
            }
            try
            {
                var seguimiento = await _context.Pedidos.FirstAsync(i => i.InvoiceNumber == id);
                if (seguimiento == null)
                {
                    return NotFound();
                }

                if (seguimiento.PedFase == 'E' || seguimiento.PedFase == 'N' || seguimiento.PedFechaFin == null)
                {
                    return Forbid();
                }

                if (_entrega.Estado)
                {
                    seguimiento.PedFase = 'E';
                    seguimiento.PedFechaEntrega = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);

                    if (!_entrega.PersonaPresente)
                    {
                        seguimiento.PedDireccionEntrega = _entrega.Direccion;
                    }
                    else
                    {
                        var entrega = new Entregas
                        {
                            PedId = seguimiento.PedId,
                            EntCelular = _entrega.Celular,
                            EntParentesco = _entrega.Parentesco,
                            EntPerApellidos = _entrega.Apellidos,
                            EntPerNombres = _entrega.Nombres,
                            EntPerIdentificacion = _entrega.Identificacion
                        };
                        _context.Add(entrega);
                    }

                    await _context.SaveChangesAsync();

                    _flashMessage.Confirmation($"Pedido con Factura Nro {seguimiento.InvoiceNumber} entregado.");
                }
                else
                {
                    seguimiento.PedFase = 'N';
                    seguimiento.PedDescripcion = _entrega.Descripcion;

                    await _context.SaveChangesAsync();

                    _flashMessage.Confirmation($"Pedido con Factura Nro {seguimiento.InvoiceNumber} registrado como entregado.");
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                _flashMessage.Danger($"Error al registrar la entrega.");
            }
            return View(_entrega);
        }
    }
}