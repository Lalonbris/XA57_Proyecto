using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XA57_Proyecto.Application.Interfaces;
using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly ITipoProductoService _tipoProductoService;
        private readonly ILineaService _lineaService;
        private readonly IPedidoService _pedidoService;
        private readonly IUsuarioAdminService _usuarioAdminService;

        public AdminController(
            IProductoService productoService,
            ITipoProductoService tipoProductoService,
            ILineaService lineaService,
            IPedidoService pedidoService,
            IUsuarioAdminService usuarioAdminService)
        {
            _productoService = productoService;
            _tipoProductoService = tipoProductoService;
            _lineaService = lineaService;
            _pedidoService = pedidoService;
            _usuarioAdminService = usuarioAdminService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        #region Productos
        [HttpGet]
        public async Task<IActionResult> Productos()
        {
            var productos = await _productoService.ObtenerTodosAsync();
            return View(productos);
        }

        [HttpGet]
        public async Task<IActionResult> ProductoDetalle(int id)
        {
            var producto = await _productoService.ObtenerPorIdAsync(id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        [HttpPost]
        public async Task<IActionResult> CrearProducto([FromBody] Producto producto)
        {
            var nuevo = await _productoService.AgregarAsync(producto);
            return Ok(nuevo);
        }

        [HttpPut]
        public async Task<IActionResult> EditarProducto(int id, [FromBody] Producto producto)
        {
            if (id != producto.Id) return BadRequest();
            await _productoService.ActualizarAsync(producto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            await _productoService.EliminarAsync(id);
            return Ok();
        }
        #endregion

        #region Tipos de Producto
        [HttpGet]
        public async Task<IActionResult> TiposProducto()
        {
            var tipos = await _tipoProductoService.ObtenerTodosAsync();
            return View(tipos);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTiposProducto()
        {
            var tipos = await _tipoProductoService.ObtenerTodosAsync();
            return Ok(tipos.Select(t => new { id = t.Id, nombre = t.Nombre }));
        }

        [HttpPost]
        public async Task<IActionResult> CrearTipoProducto([FromBody] TipoProducto tipoProducto)
        {
            var nuevo = await _tipoProductoService.AgregarAsync(tipoProducto);
            return Ok(nuevo);
        }

        [HttpPut]
        public async Task<IActionResult> EditarTipoProducto(int id, [FromBody] TipoProducto tipoProducto)
        {
            if (id != tipoProducto.Id) return BadRequest();
            await _tipoProductoService.ActualizarAsync(tipoProducto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarTipoProducto(int id)
        {
            await _tipoProductoService.EliminarAsync(id);
            return Ok();
        }
        #endregion

        #region Lineas
        [HttpGet]
        public async Task<IActionResult> Lineas()
        {
            var lineas = await _lineaService.ObtenerTodosAsync();
            return View(lineas);
        }

        [HttpPost]
        public async Task<IActionResult> CrearLinea([FromBody] Linea linea)
        {
            var nueva = await _lineaService.AgregarAsync(linea);
            return Ok(nueva);
        }

        [HttpPut]
        public async Task<IActionResult> EditarLinea(int id, [FromBody] Linea linea)
        {
            if (id != linea.Id) return BadRequest();
            await _lineaService.ActualizarAsync(linea);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarLinea(int id)
        {
            await _lineaService.EliminarAsync(id);
            return Ok();
        }
        #endregion

        #region Pedidos
        [HttpGet]
        public async Task<IActionResult> Pedidos()
        {
            var pedidos = await _pedidoService.ObtenerTodosAsync();
            return View(pedidos);
        }

        [HttpGet]
        public async Task<IActionResult> PedidoDetalle(int id)
        {
            var pedido = await _pedidoService.ObtenerPorIdAsync(id);
            if (pedido == null) return NotFound();
            return View(pedido);
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarEstadoPedido(int id, [FromBody] string estado)
        {
            await _pedidoService.ActualizarEstadoAsync(id, estado);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarPedido(int id)
        {
            await _pedidoService.EliminarItemAsync(id);
            return Ok();
        }
        #endregion

        #region Usuarios
        [HttpGet]
        public async Task<IActionResult> Usuarios()
        {
            var usuarios = await _usuarioAdminService.ObtenerTodosAsync();
            return View(usuarios);
        }

        [HttpGet]
        public async Task<IActionResult> UsuarioDetalle(string id)
        {
            var usuario = await _usuarioAdminService.ObtenerPorIdAsync(id);
            if (usuario == null) return NotFound();
            var roles = await _usuarioAdminService.ObtenerRolesAsync(id);
            ViewBag.Roles = roles;
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> CrearUsuario([FromBody] ApplicationUser usuario, string password)
        {
            var resultado = await _usuarioAdminService.CrearUsuarioAsync(usuario, password);
            if (!resultado.Succeeded)
            {
                return BadRequest(resultado.Errors);
            }
            return Ok(usuario);
        }

        [HttpPut]
        public async Task<IActionResult> EditarUsuario(string id, [FromBody] ApplicationUser usuario)
        {
            if (id != usuario.Id) return BadRequest();
            var resultado = await _usuarioAdminService.ActualizarUsuarioAsync(usuario);
            if (!resultado.Succeeded)
            {
                return BadRequest(resultado.Errors);
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarUsuario(string id)
        {
            var resultado = await _usuarioAdminService.EliminarUsuarioAsync(id);
            if (!resultado.Succeeded)
            {
                return BadRequest(resultado.Errors);
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AgregarRolUsuario(string usuarioId, string rol)
        {
            var resultado = await _usuarioAdminService.AgregarRolAsync(usuarioId, rol);
            if (!resultado.Succeeded)
            {
                return BadRequest(resultado.Errors);
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> QuitarRolUsuario(string usuarioId, string rol)
        {
            var resultado = await _usuarioAdminService.QuitarRolAsync(usuarioId, rol);
            if (!resultado.Succeeded)
            {
                return BadRequest(resultado.Errors);
            }
            return Ok();
        }
        #endregion
    }
}