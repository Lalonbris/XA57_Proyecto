using Microsoft.AspNetCore.Mvc;
using XA57_Proyecto.Application.Interfaces;

namespace XA57_Proyecto.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly ITipoProductoService _tipoProductoService;
        private readonly IProductoService _productoService;

        public CatalogoController(ITipoProductoService tipoProductoService, IProductoService productoService)
        {
            _tipoProductoService = tipoProductoService;
            _productoService = productoService;
        }

        public async Task<IActionResult> Index()
        {
            var tipos = await _tipoProductoService.ObtenerTodosAsync();
            return View(tipos);
        }

        public async Task<IActionResult> PorTipo(int id)
        {
            var productos = await _productoService.ObtenerPorTipoAsync(id);
            var tipo = await _tipoProductoService.ObtenerPorIdAsync(id);
            ViewBag.TipoProducto = tipo;
            return View(productos);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var tipos = await _tipoProductoService.ObtenerTodosAsync();
            return Json(tipos);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var tipo = await _tipoProductoService.ObtenerPorIdAsync(id);
            if (tipo == null) return NotFound();
            return Json(tipo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Domain.Entities.TipoProducto tipoProducto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var nuevo = await _tipoProductoService.AgregarAsync(tipoProducto);
            return Ok(nuevo);
        }

        [HttpPut]
        public async Task<IActionResult> Editar(int id, [FromBody] Domain.Entities.TipoProducto tipoProducto)
        {
            if (id != tipoProducto.Id) return BadRequest();
            await _tipoProductoService.ActualizarAsync(tipoProducto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _tipoProductoService.EliminarAsync(id);
            return Ok();
        }
    }
}