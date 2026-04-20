using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XA57_Proyecto.Application.Interfaces;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Infrastructure.Repositories.Interfaces;

namespace XA57_Proyecto.Controllers
{
    [Route("Admin")]
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        private readonly IPedidoRepository _pedidoRepo;
        private static readonly Dictionary<string, List<string>> _transicionesValidas = new()
        {
            { "Recibido", new List<string> { "En producción" } },
            { "En producción", new List<string> { "Enviado" } },
            { "Enviado", new List<string> { "Entregado" } },
            { "Entregado", new List<string>() }
        };

        public AdminController(IPedidoRepository pedidoRepo)
        {
            _pedidoRepo = pedidoRepo;
        }

        [HttpGet("Pedidos")]
        public async Task<IActionResult> Index()
        {
            var pedidos = await _pedidoRepo.ObtenerConProductosAsync();
            return View(pedidos);
        }

        [HttpPost("ActualizarEstado")]
        public async Task<IActionResult> ActualizarEstado([FromForm] int pedidoId, [FromForm] string nuevoEstado)
        {
            var pedido = await _pedidoRepo.ObtenerPorIdAsync(pedidoId);
            if (pedido == null)
            {
                return NotFound(new { mensaje = "Pedido no encontrado." });
            }

            if (!EsTransicionValida(pedido.Estado, nuevoEstado))
            {
                return BadRequest(new { mensaje = "Transición de estado no permitida." });
            }

            pedido.Estado = nuevoEstado;
            await _pedidoRepo.ActualizarAsync(pedido);

            return Ok(new { mensaje = "Estado actualizado.", estado = nuevoEstado });
        }

        private bool EsTransicionValida(string actual, string nuevo)
        {
            if (!_transicionesValidas.TryGetValue(actual, out var siguiente))
                return false;
            return siguiente.Contains(nuevo);
        }
    }
}