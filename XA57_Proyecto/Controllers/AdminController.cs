using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XA57_Proyecto.Application.Interfaces;
using XA57_Proyecto.Application.Dtos;
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
        private readonly IModeloAutobusService _modeloService;

        public AdminController(
            IProductoService productoService,
            ITipoProductoService tipoProductoService,
            ILineaService lineaService,
            IPedidoService pedidoService,
            IUsuarioAdminService usuarioAdminService,
            IModeloAutobusService modeloService)
        {
            _productoService = productoService;
            _tipoProductoService = tipoProductoService;
            _lineaService = lineaService;
            _pedidoService = pedidoService;
            _usuarioAdminService = usuarioAdminService;
            _modeloService = modeloService;
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
            var productos = await _productoService.ObtenerTodosAdminAsync();
            return View(productos);
        }

        [HttpGet]
        public async Task<IActionResult> ProductoDetalle(int id)
        {
            var producto = await _productoService.ObtenerPorIdAsync(id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerProducto(int id)
        {
            var producto = await _productoService.ObtenerPorIdAsync(id);
            if (producto == null) return NotFound();
            return Ok(producto);
        }

        [HttpPost]
        public async Task<IActionResult> CrearProducto([FromBody] Producto producto)
        {
            try
            {
                var nuevo = await _productoService.AgregarAsync(producto);
                return Ok(nuevo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditarProducto(int id, [FromBody] Producto producto)
        {
            try
            {
                if (id != producto.Id) return BadRequest(new { message = "ID no coincide" });
                await _productoService.ActualizarAsync(producto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            try
            {
                await _productoService.EliminarAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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

        [HttpGet]
        public async Task<IActionResult> ObtenerTipoProducto(int id)
        {
            var tipo = await _tipoProductoService.ObtenerPorIdAsync(id);
            if (tipo == null) return NotFound();
            return Ok(tipo);
        }

        [HttpPost]
        public async Task<IActionResult> CrearTipoProducto([FromBody] TipoProducto tipoProducto)
        {
            try
            {
                var nuevo = await _tipoProductoService.AgregarAsync(tipoProducto);
                return Ok(nuevo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditarTipoProducto(int id, [FromBody] TipoProducto tipoProducto)
        {
            try
            {
                if (id != tipoProducto.Id) return BadRequest(new { message = "ID no coincide" });
                await _tipoProductoService.ActualizarAsync(tipoProducto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarTipoProducto(int id)
        {
            try
            {
                await _tipoProductoService.EliminarAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region Lineas
        [HttpGet]
        public async Task<IActionResult> Lineas()
        {
            var lineas = await _lineaService.ObtenerTodosAsync();
            return View(lineas);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerLineas()
        {
            var lineas = await _lineaService.ObtenerTodosAsync();
            return Ok(lineas.Where(l => l.Activa).Select(l => new { id = l.Id, nombre = l.Nombre }));
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerLinea(int id)
        {
            var linea = await _lineaService.ObtenerPorIdAsync(id);
            if (linea == null) return NotFound();
            return Ok(linea);
        }

        [HttpPost]
        public async Task<IActionResult> CrearLinea([FromBody] Linea linea)
        {
            try
            {
                var nueva = await _lineaService.AgregarAsync(linea);
                return Ok(nueva);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditarLinea(int id, [FromBody] Linea linea)
        {
            try
            {
                if (id != linea.Id) return BadRequest(new { message = "ID no coincide" });
                await _lineaService.ActualizarAsync(linea);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarLinea(int id)
        {
            try
            {
                await _lineaService.EliminarAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region Modelos de Autobús
        [HttpGet]
        public async Task<IActionResult> Modelos()
        {
            var modelos = await _modeloService.ObtenerTodosAsync();
            return View(modelos);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerModelo(int id)
        {
            var modelo = await _modeloService.ObtenerPorIdAsync(id);
            if (modelo == null) return NotFound();
            return Ok(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> CrearModelo([FromBody] ModeloAutobus modelo)
        {
            try
            {
                var nuevo = await _modeloService.AgregarAsync(modelo);
                return Ok(nuevo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditarModelo(int id, [FromBody] ModeloAutobus modelo)
        {
            try
            {
                if (id != modelo.Id) return BadRequest(new { message = "ID no coincide" });
                await _modeloService.ActualizarAsync(modelo);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarModelo(int id)
        {
            try
            {
                await _modeloService.EliminarAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region Pedidos
        [HttpGet]
        public async Task<IActionResult> Pedidos(string? estado)
        {
            var pedidos = await _pedidoService.ObtenerTodosAsync();
            if (!string.IsNullOrEmpty(estado))
            {
                pedidos = pedidos.Where(p => p.Estado == estado).ToList();
            }
            return View(pedidos);
        }

        [HttpGet]
        public async Task<IActionResult> PedidoDetalle(int id)
        {
            var pedido = await _pedidoService.ObtenerPorIdAsync(id);
            if (pedido == null) return NotFound();
            return Ok(pedido);
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarEstadoPedido(int id, [FromBody] string nuevoEstado)
        {
            try
            {
                var pedido = await _pedidoService.ObtenerPorIdAsync(id);
                if (pedido == null) return NotFound(new { message = "Pedido no encontrado" });

                if (!EsTransicionValida(pedido.Estado, nuevoEstado))
                {
                    return BadRequest(new { message = $"Transición de estado inválida: {pedido.Estado} -> {nuevoEstado}" });
                }

                await _pedidoService.ActualizarEstadoAsync(id, nuevoEstado);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private static readonly Dictionary<string, string> _transicionesValidas = new()
        {
            ["Recibido"] = "En producción",
            ["En producción"] = "Enviado",
            ["Enviado"] = "Entregado"
        };

        private bool EsTransicionValida(string actual, string nuevo)
        {
            if (actual == nuevo) return true; // No hay cambio
            return _transicionesValidas.TryGetValue(actual, out var siguiente) && siguiente == nuevo;
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarPedido(int id)
        {
            try
            {
                await _pedidoService.EliminarItemAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListaPicking(int id)
        {
            var pedido = await _pedidoService.ObtenerPorIdAsync(id);
            if (pedido == null) return NotFound();
            return View(pedido);
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

        [HttpGet]
        public async Task<IActionResult> ObtenerUsuario(string id)
        {
            var usuario = await _usuarioAdminService.ObtenerPorIdAsync(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> CrearUsuario([FromBody] UsuarioCreacionDto dto)
        {
            try
            {
                if (dto == null || dto.Usuario == null)
                {
                    return BadRequest(new { message = "Datos de usuario inválidos." });
                }

                // Asignar un nuevo ID único al crear el usuario para evitar conflictos de llave primaria.
                dto.Usuario.Id = Guid.NewGuid().ToString();

                var resultado = await _usuarioAdminService.CrearUsuarioAsync(dto.Usuario, dto.Password);
                if (!resultado.Succeeded)
                {
                    return BadRequest(resultado.Errors);
                }

                var usuarioCreado = await _usuarioAdminService.ObtenerPorEmailAsync(dto.Usuario.Email);
                if(usuarioCreado == null)
                {
                    // Manejar el caso improbable de que el usuario no se encuentre después de crearlo.
                    return StatusCode(500, new { message = "Error al recuperar el usuario después de la creación." });
                }

                if (!string.IsNullOrEmpty(dto.Rol))
                {
                    var rolResult = await _usuarioAdminService.AgregarRolAsync(usuarioCreado.Id, dto.Rol);
                    if (!rolResult.Succeeded)
                    {
                        // Opcional: considerar eliminar el usuario recién creado si la asignación de rol falla.
                        await _usuarioAdminService.EliminarUsuarioAsync(usuarioCreado.Id);
                        return BadRequest(rolResult.Errors);
                    }
                }
                
                return Ok(usuarioCreado);
            }
            catch (Exception ex)
            {
                // Devolver el mensaje de la excepción para depuración.
                return StatusCode(500, new { message = ex.Message, detail = ex.ToString() });
            }
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