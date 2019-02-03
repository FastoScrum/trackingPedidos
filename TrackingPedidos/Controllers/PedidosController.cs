using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackingPedidos.Models;
using TrackingPedidos.Services;

namespace TrackingPedidos.Controllers
{
    [Authorize]
    public class PedidosController : Controller
    {
        private readonly ApiService _api;

        public PedidosController()
        {
            _api = new ApiService();
        }

        public async Task<IActionResult> Index()
        {
            var response = await _api.GetList<Invoice>("http://facturacion-utn-amazon.herokuapp.com", "/api/invoices-utn");
            if (response.IsSuccess)
            {
                return View((List<Invoice>)response.Result);
            }
            return View();
        }
    }
}