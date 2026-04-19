using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using XA57_Proyecto.Application.Interfaces;
using XA57_Proyecto.ViewModels;

namespace XA57_Proyecto.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductoService _productoService;

        public HomeController(ILogger<HomeController> logger, IProductoService productoService)
        {
            _logger = logger;
            _productoService = productoService;
        }

        public async Task<IActionResult> Index()
        {
            var productosRecientes = await _productoService.ObtenerTodosAsync();
            return View(productosRecientes.OrderByDescending(p => p.Id).Take(4).ToList());
        }

        public async Task<IActionResult> CatalogoPorTipo(int tipo)
        {
            List<Domain.Entities.Producto> resultado;
            if (tipo <= 0)
            {
                resultado = await _productoService.ObtenerTodosAsync();
                return View("Catalogo", resultado);
            }
            resultado = await _productoService.ObtenerPorTipoAsync(tipo);
            return View("Catalogo", resultado);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Catalogo()
        {
            var productos = await _productoService.ObtenerTodosAsync();
            return View(productos);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
