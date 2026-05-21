using Microsoft.AspNetCore.Mvc;
using XA57_Proyecto.Application.DTOs;
using XA57_Proyecto.Application.Interfaces;
using XA57_Proyecto.ViewModels;

namespace XA57_Proyecto.Controllers
{
    [Route("Carrito")]
    public class CarritoController : Controller
    {
        private readonly IPedidoService _pedidoService;

        public CarritoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var items = await _pedidoService.ObtenerCarritoAsync();
            var viewModel = new CarritoViewModel { Items = items };
            return View(viewModel);
        }

        [HttpPost("Agregar")]
        public async Task<IActionResult> Agregar([FromBody] CarritoItemDto item)
        {
            if (item == null || item.ProductoId == 0)
                return BadRequest(new { mensaje = "Datos inválidos." });

            var resultado = await _pedidoService.AgregarAsync(item);
            return Ok(new { mensaje = resultado.Mensaje, pedidoId = resultado.PedidoId });
        }

        [HttpPost("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _pedidoService.EliminarItemAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Cantidad")]
        public async Task<IActionResult> Cantidad()
        {
            var count = await _pedidoService.ContarItemsAsync();
            return Ok(count);
        }
    }
}
