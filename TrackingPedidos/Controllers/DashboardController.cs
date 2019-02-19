using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TrackingPedidos.Models;

namespace TrackingPedidos.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly TrackingContext _context;

        public DashboardController(TrackingContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Administrador"))
            {
                ViewData["Pendientes"] = _context.Pedidos.Where(i => i.PedFase.ToString() == "P").Count();
                ViewData["EnCamino"] = _context.Pedidos.Where(i => i.PedFase.ToString() == "C").Count();
                ViewData["EnDistribuidora"] = _context.Pedidos.Where(i => i.PedFase.ToString() == "F").Count();
                ViewData["Entregados"] = _context.Pedidos.Where(i => i.PedFase.ToString() == "E").Count();
                ViewData["NoEntregados"] = _context.Pedidos.Where(i => i.PedFase.ToString() == "N").Count();
            }
            return View();
        }
    }
}