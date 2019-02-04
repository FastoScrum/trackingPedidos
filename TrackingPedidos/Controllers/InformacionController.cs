using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TrackingPedidos.Models;

namespace TrackingPedidos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformacionController : ControllerBase
    {
        private readonly TrackingContext _context;

        public InformacionController(TrackingContext context)
        {
            _context = context;
        }

        // GET: api/Informacion/548484848
        [HttpGet("{factura}")]
        public async Task<IActionResult> FindByFactura(string factura)
        {
            try
            {
                var pedido = await _context.Pedidos.FirstAsync(i => i.InvoiceNumber == factura);
                if (pedido == null)
                {
                    return NotFound();
                }
                return Ok(pedido);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}