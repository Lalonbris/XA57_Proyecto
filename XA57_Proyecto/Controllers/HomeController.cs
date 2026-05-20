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
            try
            {
                var productosRecientes = await _productoService.ObtenerTodosAsync();
                return View(productosRecientes.OrderByDescending(p => p.Id).Take(4).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos para la página de inicio.");
                // Retornar vista con lista vacía para que la página cargue aunque la BD no responda
                return View(new List<Domain.Entities.Producto>());
            }
        }

        public async Task<IActionResult> CatalogoPorTipo(int tipo)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener catálogo por tipo.");
                return View("Catalogo", new List<Domain.Entities.Producto>());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Catalogo()
        {
            try
            {
                var productos = await _productoService.ObtenerTodosAsync();
                return View(productos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener catálogo.");
                return View(new List<Domain.Entities.Producto>());
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
