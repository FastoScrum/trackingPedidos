using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TrackingPedidos.Data;
using TrackingPedidos.Models;

namespace TrackingPedidos.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class EnviosController : Controller
    {
        private readonly TrackingContext _context;
        private readonly ApplicationDbContext _contextIdentity;

        public EnviosController(TrackingContext context, ApplicationDbContext contextIdentity)
        {
            _context = context;
            _contextIdentity = contextIdentity;
        }

        public async Task<IActionResult> Index()
        {
            var email = User.Identity.Name;
            var seguimiento = await _context.Pedidos
                    .Where(i => i.ClienteEmail == email)
                    .OrderByDescending(i => i.PedFechaDespachado)
                    .AsNoTracking().ToListAsync();
            return View(seguimiento);
        }

        public async Task<IActionResult> Details(string id)
        {
            var pedido = await _context.Pedidos.Include(i => i.Entregas).FirstOrDefaultAsync(i => i.InvoiceNumber == id);
            if (pedido == null)
            {
                return NotFound();
            }

            var email = User.Identity.Name;
            if (pedido.ClienteEmail != email)
            {
                return Forbid();
            }

            if (pedido.Despachador != null)
            {
                ViewData["Despachador"] = await _contextIdentity.ApplicationUser.FirstOrDefaultAsync(i => i.UserName == pedido.Despachador);
            }

            if (pedido.Transportista != null)
            {
                ViewData["Transportista"] = await _contextIdentity.ApplicationUser.FirstOrDefaultAsync(i => i.UserName == pedido.Transportista);
            }

            if (pedido.Distribuidor != null)
            {
                ViewData["Distribuidor"] = await _contextIdentity.ApplicationUser.FirstOrDefaultAsync(i => i.UserName == pedido.Distribuidor);
            }

            if (pedido.Mensajero != null)
            {
                ViewData["Mensajero"] = await _contextIdentity.ApplicationUser.FirstOrDefaultAsync(i => i.UserName == pedido.Mensajero);
            }

            return View(pedido);
        }
    }
}