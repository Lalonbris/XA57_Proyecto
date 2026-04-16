using Microsoft.AspNetCore.Mvc;
using XA57_Proyecto.Application.Interfaces;

namespace XA57_Proyecto.Controllers
{
    [Route("api/configurador")]
    [ApiController]
    public class ConfiguradorApiController : ControllerBase
    {
        private readonly IModeloAutobusService _modeloService;
        private readonly ILineaService _lineaService;
        private readonly IProductoService _productoService;

        public ConfiguradorApiController(
            IModeloAutobusService modeloService,
            ILineaService lineaService,
            IProductoService productoService)
        {
            _modeloService = modeloService;
            _lineaService = lineaService;
            _productoService = productoService;
        }

        // GET /api/configurador/modelos
        [HttpGet("modelos")]
        public async Task<IActionResult> GetModelos()
        {
            var modelos = await _modeloService.GetAllActiveAsync();
            return Ok(modelos.Select(m => new { m.Id, m.Nombre, m.Fabricante }));
        }

        // GET /api/configurador/lineas
        [HttpGet("lineas")]
        public async Task<IActionResult> GetLineas()
        {
            var lineas = await _lineaService.GetAllActiveAsync();
            return Ok(lineas.Select(l => new
            {
                l.Id,
                l.Nombre,
                l.ColorPrimario,
                l.ColorSecundario,
                l.NombreOperador,
                l.LogoUrl
            }));
        }

        // GET /api/configurador/producto/{id}
        [HttpGet("producto/{id}")]
        public async Task<IActionResult> GetProducto(int id)
        {
            var producto = await _productoService.GetByIdAsync(id);
            if (producto == null) return NotFound();

            return Ok(new
            {
                producto.Id,
                producto.Nombre,
                producto.Precio,
                producto.ImagenUrl,
                tipoProducto = producto.TipoProducto == null ? null : new
                {
                    producto.TipoProducto.Id,
                    producto.TipoProducto.Nombre,
                    producto.TipoProducto.MaxCaracteres,
                    producto.TipoProducto.PermiteNombre,
                    producto.TipoProducto.PermiteNumeroEconomico,
                    producto.TipoProducto.PermiteRuta
                }
            });
        }
    }
}