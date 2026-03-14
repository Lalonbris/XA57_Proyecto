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

        public IActionResult Index()
        {
            return View();
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
