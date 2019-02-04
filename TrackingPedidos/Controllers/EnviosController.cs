using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TrackingPedidos.Models;

namespace TrackingPedidos.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class EnviosController : Controller
    {
        private readonly TrackingContext _context;

        public EnviosController(TrackingContext context)
        {
            _context = context;
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

            return View(pedido);
        }
    }
}