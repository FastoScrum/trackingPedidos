using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackingPedidos.Models;
using TrackingPedidos.Services;
using TrackingPedidos.ViewModels;
using Vereyon.Web;

namespace TrackingPedidos.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class PedidosController : Controller
    {
        private readonly ApiService _api;
        private readonly TrackingContext _context;
        public IFlashMessage _flashMessage { get; private set; }

        public PedidosController(TrackingContext context, IFlashMessage flashMessage)
        {
            _api = new ApiService();
            _context = context;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            var email = User.Identity.Name;
            var response = await _api.GetList<Invoice>("http://facturacion-utn-amazon.herokuapp.com", "/api/invoices-utn");
            if (response.IsSuccess)
            {
                //var pedidos = await _context.Pedidos
                //    .Where(i => i.ClienteEmail == email)
                //    .AsNoTracking().ToListAsync();

                //var registrados = new List<string>();
                //foreach (var item in pedidos)
                //{
                //    registrados.Add(item.InvoiceNumber);
                //}

                //ViewData["Registados"] = registrados;

                var lista = (List<Invoice>)response.Result;
                return View(lista.Where(i => i.user_client.email == email));
            }
            return View();
        }

        // GET: Pedidos/Send
        public async Task<IActionResult> Send(string id)
        {
            var pedido = await _context.Pedidos.FirstOrDefaultAsync(i => i.InvoiceNumber == id);
            if (pedido != null)
            {
                return Forbid();
            }
            var invoice = await _api.GetInvoice("http://facturacion-utn-amazon.herokuapp.com", "/api/invoice", id);
            if (invoice == null)
            {
                return NotFound();
            }

            var email = User.Identity.Name;

            if (invoice.user_client.email != email)
            {
                return Forbid();
            }

            var seguimiento = new SeguimientoVM
            {
                InvoiceNumber = id,
                PedTotal = decimal.Parse(invoice.total)
            };
            return View(seguimiento);
        }

        // POST: Pedidos/Send/00001-001-001
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(string id, SeguimientoVM seguimiento)
        {
            if (ModelState.IsValid)
            {
                var invoice = await _api.GetInvoice("http://facturacion-utn-amazon.herokuapp.com", "/api/invoice", id);
                if (invoice == null)
                {
                    return NotFound();
                }

                var email = User.Identity.Name;

                if (invoice.user_client.email != email)
                {
                    return Forbid();
                }

                try
                {
                    var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
                    var tipoT = seguimiento.PedTipoEnvio.Equals("T");
                    var regalo = seguimiento.PedRegalo.Equals("S");

                    var extra = 0;
                    if (!tipoT)
                    {
                        extra = 5;
                    }

                    if (regalo)
                    {
                        extra += 10;
                    }

                    var pedido = new Pedidos
                    {
                        InvoiceNumber = id,
                        ClienteEmail = email,
                        PedTotal = decimal.Parse(invoice.total),
                        PedFechaEnvio = now,
                        PedFase = 'P',
                        PedLugarOrigen = seguimiento.PedLugarOrigen,
                        PedLugarDestino = seguimiento.PedLugarDestino,
                        PedEnvioEstandar = tipoT,
                        PedCostoExtra = extra,
                        PedRegalo = regalo,
                        PedTarjeta = seguimiento.PedTarjeta
                    };

                    _context.Add(pedido);
                    await _context.SaveChangesAsync();
                    _flashMessage.Confirmation("Pedido enviado.");

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            seguimiento.InvoiceNumber = id;

            return View(seguimiento);
        }
    }
}