using Microsoft.AspNetCore.Mvc;
using XA57_Proyecto.Application.Interfaces;

namespace XA57_Proyecto.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductoService _productoService;

        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var producto = await _productoService.GetByIdAsync(id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        public async Task<IActionResult> Catalogo()
        {
            var productos = await _productoService.GetAllAsync();
            return View(productos);
        }
    }
}
