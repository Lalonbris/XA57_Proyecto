using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Moq;
using XA57_Proyecto.Application.Exceptions;
using XA57_Proyecto.Application.Interfaces;
using XA57_Proyecto.Controllers;
using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Tests.Controllers;

public class AdminControllerTests
{
    private readonly Mock<IProductoService> _productoServiceMock;
    private readonly Mock<ITipoProductoService> _tipoProductoServiceMock;
    private readonly Mock<ILineaService> _lineaServiceMock;
    private readonly Mock<IPedidoService> _pedidoServiceMock;
    private readonly Mock<IModeloAutobusService> _modeloServiceMock;
    private readonly Mock<IUsuarioAdminService> _usuarioAdminServiceMock;
    private readonly AdminController _controller;

    private static string? GetJsonMessage(IActionResult result)
    {
        if (result is ObjectResult or && or.Value != null)
        {
            var json = JsonSerializer.Serialize(or.Value);
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.TryGetProperty("message", out var msg) ? msg.GetString() : null;
        }
        return null;
    }

    public AdminControllerTests()
    {
        _productoServiceMock = new Mock<IProductoService>();
        _tipoProductoServiceMock = new Mock<ITipoProductoService>();
        _lineaServiceMock = new Mock<ILineaService>();
        _pedidoServiceMock = new Mock<IPedidoService>();
        _modeloServiceMock = new Mock<IModeloAutobusService>();
        _usuarioAdminServiceMock = new Mock<IUsuarioAdminService>();

        _controller = new AdminController(
            _productoServiceMock.Object,
            _tipoProductoServiceMock.Object,
            _lineaServiceMock.Object,
            _pedidoServiceMock.Object,
            _usuarioAdminServiceMock.Object,
            _modeloServiceMock.Object);
    }

    // ============ Index ============

    [Fact]
    public void Index_RetornaVista()
    {
        var result = _controller.Index();
        Assert.IsType<ViewResult>(result);
    }

    // ============ Productos ============

    [Fact]
    public async Task Productos_RetornaVistaConProductos()
    {
        var productos = new List<Producto> { new() { Id = 1, Nombre = "P1" } };
        _productoServiceMock.Setup(s => s.ObtenerTodosAdminAsync()).ReturnsAsync(productos);

        var result = await _controller.Productos();
        var view = Assert.IsType<ViewResult>(result);
        Assert.Same(productos, view.Model);
    }

    [Fact]
    public async Task ProductoDetalle_Existe_RetornaVista()
    {
        var producto = new Producto { Id = 1, Nombre = "P1" };
        _productoServiceMock.Setup(s => s.ObtenerPorIdAsync(1)).ReturnsAsync(producto);

        var result = await _controller.ProductoDetalle(1);
        var view = Assert.IsType<ViewResult>(result);
        Assert.Same(producto, view.Model);
    }

    [Fact]
    public async Task ProductoDetalle_NoExiste_RetornaNotFound()
    {
        _productoServiceMock.Setup(s => s.ObtenerPorIdAsync(999)).ReturnsAsync((Producto?)null);
        var result = await _controller.ProductoDetalle(999);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task ObtenerProducto_Existe_RetornaOk()
    {
        var producto = new Producto { Id = 1, Nombre = "P1" };
        _productoServiceMock.Setup(s => s.ObtenerPorIdAsync(1)).ReturnsAsync(producto);

        var result = await _controller.ObtenerProducto(1);
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(producto, ok.Value);
    }

    [Fact]
    public async Task ObtenerProducto_NoExiste_RetornaNotFound()
    {
        _productoServiceMock.Setup(s => s.ObtenerPorIdAsync(999)).ReturnsAsync((Producto?)null);
        var result = await _controller.ObtenerProducto(999);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CrearProducto_Valido_RetornaOk()
    {
        var producto = new Producto { Nombre = "Nuevo", Precio = 100 };
        _productoServiceMock.Setup(s => s.AgregarAsync(producto)).ReturnsAsync(producto);

        var result = await _controller.CrearProducto(producto);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task CrearProducto_Error_RetornaBadRequest()
    {
        var producto = new Producto { Nombre = "", Precio = 0 };
        _productoServiceMock.Setup(s => s.AgregarAsync(producto))
            .ThrowsAsync(new ValidationException("Nombre inválido"));

        var result = await _controller.CrearProducto(producto);
        var br = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Nombre inválido", GetJsonMessage(result));
    }

    [Fact]
    public async Task EditarProducto_IdNoCoincide_RetornaBadRequest()
    {
        var producto = new Producto { Id = 2, Nombre = "X", Precio = 100 };
        var result = await _controller.EditarProducto(1, producto);
        var br = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("no coincide", GetJsonMessage(result));
    }

    [Fact]
    public async Task EditarProducto_Valido_RetornaOk()
    {
        var producto = new Producto { Id = 1, Nombre = "X", Precio = 100 };
        var result = await _controller.EditarProducto(1, producto);
        Assert.IsType<OkResult>(result);
        _productoServiceMock.Verify(s => s.ActualizarAsync(producto), Times.Once);
    }

    [Fact]
    public async Task EliminarProducto_Valido_RetornaOk()
    {
        var result = await _controller.EliminarProducto(1);
        Assert.IsType<OkResult>(result);
        _productoServiceMock.Verify(s => s.EliminarAsync(1), Times.Once);
    }

    [Fact]
    public async Task EliminarProducto_Error_RetornaBadRequest()
    {
        _productoServiceMock.Setup(s => s.EliminarAsync(1))
            .ThrowsAsync(new ValidationException("No existe"));
        var result = await _controller.EliminarProducto(1);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    // ============ Tipos de Producto ============

    [Fact]
    public async Task TiposProducto_RetornaVista()
    {
        var tipos = new List<TipoProducto> { new() { Id = 1, Nombre = "T1" } };
        _tipoProductoServiceMock.Setup(s => s.ObtenerTodosAsync()).ReturnsAsync(tipos);

        var result = await _controller.TiposProducto();
        var view = Assert.IsType<ViewResult>(result);
        Assert.Same(tipos, view.Model);
    }

    [Fact]
    public async Task ObtenerTiposProducto_RetornaJson()
    {
        var tipos = new List<TipoProducto> { new() { Id = 1, Nombre = "T1" } };
        _tipoProductoServiceMock.Setup(s => s.ObtenerTodosAsync()).ReturnsAsync(tipos);

        var result = await _controller.ObtenerTiposProducto();
        var ok = Assert.IsType<OkObjectResult>(result);

        var json = JsonSerializer.Serialize(ok.Value);
        using var doc = JsonDocument.Parse(json);
        Assert.Equal(1, doc.RootElement.GetArrayLength());
    }

    [Fact]
    public async Task ObtenerTipoProducto_Existe_RetornaOk()
    {
        var tipo = new TipoProducto { Id = 1, Nombre = "T1" };
        _tipoProductoServiceMock.Setup(s => s.ObtenerPorIdAsync(1)).ReturnsAsync(tipo);

        var result = await _controller.ObtenerTipoProducto(1);
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(tipo, ok.Value);
    }

    [Fact]
    public async Task CrearTipoProducto_Valido_RetornaOk()
    {
        var tipo = new TipoProducto { Nombre = "Nuevo", MaxCaracteres = 20 };
        _tipoProductoServiceMock.Setup(s => s.AgregarAsync(tipo)).ReturnsAsync(tipo);

        var result = await _controller.CrearTipoProducto(tipo);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task EditarTipoProducto_IdNoCoincide_RetornaBadRequest()
    {
        var result = await _controller.EditarTipoProducto(1, new TipoProducto { Id = 2, Nombre = "X" });
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task EliminarTipoProducto_Valido_RetornaOk()
    {
        var result = await _controller.EliminarTipoProducto(1);
        Assert.IsType<OkResult>(result);
        _tipoProductoServiceMock.Verify(s => s.EliminarAsync(1), Times.Once);
    }

    // ============ Lineas ============

    [Fact]
    public async Task Lineas_RetornaVistaConLineas()
    {
        var lineas = new List<Linea> { new() { Id = 1, Nombre = "ETN" } };
        _lineaServiceMock.Setup(s => s.ObtenerTodosAsync()).ReturnsAsync(lineas);

        var result = await _controller.Lineas();
        var view = Assert.IsType<ViewResult>(result);
        Assert.Same(lineas, view.Model);
    }

    [Fact]
    public async Task ObtenerLineas_RetornaSoloActivas()
    {
        var lineas = new List<Linea>
        {
            new() { Id = 1, Nombre = "ETN", Activa = true },
            new() { Id = 2, Nombre = "Inactiva", Activa = false }
        };
        _lineaServiceMock.Setup(s => s.ObtenerTodosAsync()).ReturnsAsync(lineas);

        var result = await _controller.ObtenerLineas();
        var ok = Assert.IsType<OkObjectResult>(result);
        var json = JsonSerializer.Serialize(ok.Value);
        using var doc = JsonDocument.Parse(json);
        Assert.Equal(1, doc.RootElement.GetArrayLength());
    }

    [Fact]
    public async Task ObtenerLinea_Existe_RetornaOk()
    {
        var linea = new Linea { Id = 1, Nombre = "ETN" };
        _lineaServiceMock.Setup(s => s.ObtenerPorIdAsync(1)).ReturnsAsync(linea);

        var result = await _controller.ObtenerLinea(1);
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(linea, ok.Value);
    }

    [Fact]
    public async Task CrearLinea_Valido_RetornaOk()
    {
        var linea = new Linea { Nombre = "Nueva", Activa = true };
        _lineaServiceMock.Setup(s => s.AgregarAsync(linea)).ReturnsAsync(linea);

        var result = await _controller.CrearLinea(linea);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task EditarLinea_IdNoCoincide_RetornaBadRequest()
    {
        var result = await _controller.EditarLinea(1, new Linea { Id = 2, Nombre = "X" });
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task EliminarLinea_Valido_RetornaOk()
    {
        var result = await _controller.EliminarLinea(1);
        Assert.IsType<OkResult>(result);
        _lineaServiceMock.Verify(s => s.EliminarAsync(1), Times.Once);
    }

    // ============ Modelos de Autobús ============

    [Fact]
    public async Task Modelos_RetornaVista()
    {
        var modelos = new List<ModeloAutobus> { new() { Id = 1, Nombre = "Irizar i8" } };
        _modeloServiceMock.Setup(s => s.ObtenerTodosAsync()).ReturnsAsync(modelos);

        var result = await _controller.Modelos();
        var view = Assert.IsType<ViewResult>(result);
        Assert.Same(modelos, view.Model);
    }

    [Fact]
    public async Task ObtenerModelo_Existe_RetornaOk()
    {
        var modelo = new ModeloAutobus { Id = 1, Nombre = "Irizar i8" };
        _modeloServiceMock.Setup(s => s.ObtenerPorIdAsync(1)).ReturnsAsync(modelo);

        var result = await _controller.ObtenerModelo(1);
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(modelo, ok.Value);
    }

    [Fact]
    public async Task CrearModelo_Valido_RetornaOk()
    {
        var modelo = new ModeloAutobus { Nombre = "Nuevo", Fabricante = "F" };
        _modeloServiceMock.Setup(s => s.AgregarAsync(modelo)).ReturnsAsync(modelo);

        var result = await _controller.CrearModelo(modelo);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task EditarModelo_IdNoCoincide_RetornaBadRequest()
    {
        var result = await _controller.EditarModelo(1, new ModeloAutobus { Id = 2, Nombre = "X", Fabricante = "Y" });
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task EliminarModelo_Valido_RetornaOk()
    {
        var result = await _controller.EliminarModelo(1);
        Assert.IsType<OkResult>(result);
        _modeloServiceMock.Verify(s => s.EliminarAsync(1), Times.Once);
    }

    // ============ Pedidos ============

    [Fact]
    public async Task Pedidos_RetornaVistaConFiltroOpcional()
    {
        var pedidos = new List<Pedido>
        {
            new() { Id = 1, Estado = "Recibido" },
            new() { Id = 2, Estado = "Enviado" }
        };
        _pedidoServiceMock.Setup(s => s.ObtenerTodosAsync()).ReturnsAsync(pedidos);

        var result = await _controller.Pedidos("Recibido");
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<List<Pedido>>(view.Model);
        Assert.Single(model);
    }

    [Fact]
    public async Task Pedidos_SinFiltro_RetornaTodos()
    {
        var pedidos = new List<Pedido> { new() { Id = 1 }, new() { Id = 2 } };
        _pedidoServiceMock.Setup(s => s.ObtenerTodosAsync()).ReturnsAsync(pedidos);

        var result = await _controller.Pedidos(null);
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<List<Pedido>>(view.Model);
        Assert.Equal(2, model.Count);
    }

    [Fact]
    public async Task PedidoDetalle_Existe_RetornaOk()
    {
        var pedido = new Pedido { Id = 1 };
        _pedidoServiceMock.Setup(s => s.ObtenerPorIdAsync(1)).ReturnsAsync(pedido);

        var result = await _controller.PedidoDetalle(1);
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(pedido, ok.Value);
    }

    [Fact]
    public async Task DetallePedido_Existe_RetornaVista()
    {
        var pedido = new Pedido { Id = 1 };
        _pedidoServiceMock.Setup(s => s.ObtenerPorIdAsync(1)).ReturnsAsync(pedido);

        var result = await _controller.DetallePedido(1);
        var view = Assert.IsType<ViewResult>(result);
        Assert.Same(pedido, view.Model);
    }

    [Fact]
    public async Task DetallePedido_NoExiste_RetornaNotFound()
    {
        _pedidoServiceMock.Setup(s => s.ObtenerPorIdAsync(999)).ReturnsAsync((Pedido?)null);
        var result = await _controller.DetallePedido(999);
        Assert.IsType<NotFoundResult>(result);
    }

    // ============ RN-11: Flujo de Estado ============

    [Fact]
    public async Task ActualizarEstadoPedido_TransicionValida_RetornaOk()
    {
        var pedido = new Pedido { Id = 1, Estado = "Recibido" };
        _pedidoServiceMock.Setup(s => s.ObtenerPorIdAsync(1)).ReturnsAsync(pedido);

        var result = await _controller.ActualizarEstadoPedido(1, "En producción");
        Assert.IsType<OkResult>(result);
        _pedidoServiceMock.Verify(s => s.ActualizarEstadoAsync(1, "En producción"), Times.Once);
    }

    [Fact]
    public async Task ActualizarEstadoPedido_TransicionInvalida_RetornaBadRequest()
    {
        var pedido = new Pedido { Id = 1, Estado = "Recibido" };
        _pedidoServiceMock.Setup(s => s.ObtenerPorIdAsync(1)).ReturnsAsync(pedido);

        var result = await _controller.ActualizarEstadoPedido(1, "Entregado");
        var br = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("inválida", GetJsonMessage(result));
    }

    [Fact]
    public async Task ActualizarEstadoPedido_MismoEstado_RetornaOk()
    {
        var pedido = new Pedido { Id = 1, Estado = "Recibido" };
        _pedidoServiceMock.Setup(s => s.ObtenerPorIdAsync(1)).ReturnsAsync(pedido);

        var result = await _controller.ActualizarEstadoPedido(1, "Recibido");
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task ActualizarEstadoPedido_PedidoNoExiste_RetornaNotFound()
    {
        _pedidoServiceMock.Setup(s => s.ObtenerPorIdAsync(999)).ReturnsAsync((Pedido?)null);

        var result = await _controller.ActualizarEstadoPedido(999, "Enviado");
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task EliminarPedido_Valido_RetornaOk()
    {
        var result = await _controller.EliminarPedido(1);
        Assert.IsType<OkResult>(result);
        _pedidoServiceMock.Verify(s => s.EliminarItemAsync(1), Times.Once);
    }

    // ============ RN-12: Lista de Picking ============

    [Fact]
    public async Task ListaPicking_PedidoExiste_RetornaVista()
    {
        var pedido = new Pedido
        {
            Id = 1,
            ProductoId = 5,
            ModeloAutobusId = 1,
            LineaId = 2,
            NombreOperador = "Juan Pérez",
            NumeroEconomico = "105",
            Ruta = "México - Guadalajara",
            Cantidad = 2,
            Producto = new Producto { Id = 5, Nombre = "Busito" },
            ModeloAutobus = new ModeloAutobus { Id = 1, Nombre = "Irizar i8", Fabricante = "Irizar" },
            Linea = new Linea { Id = 2, Nombre = "ETN", ColorPrimario = "#FF0000", ColorSecundario = "#FFF" }
        };
        _pedidoServiceMock.Setup(s => s.ObtenerPorIdAsync(1)).ReturnsAsync(pedido);

        var result = await _controller.ListaPicking(1);
        var view = Assert.IsType<ViewResult>(result);
        Assert.Same(pedido, view.Model);
    }

    [Fact]
    public async Task ListaPicking_PedidoNoExiste_RetornaNotFound()
    {
        _pedidoServiceMock.Setup(s => s.ObtenerPorIdAsync(999)).ReturnsAsync((Pedido?)null);
        var result = await _controller.ListaPicking(999);
        Assert.IsType<NotFoundResult>(result);
    }
}
