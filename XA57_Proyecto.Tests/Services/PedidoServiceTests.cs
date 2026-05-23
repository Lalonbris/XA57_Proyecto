using Microsoft.Extensions.Logging;
using Moq;
using XA57_Proyecto.Application.DTOs;
using XA57_Proyecto.Application.Exceptions;
using XA57_Proyecto.Application.Services;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Infrastructure.Repositories.Interfaces;

namespace XA57_Proyecto.Tests.Services;

public class PedidoServiceTests
{
    private readonly Mock<IPedidoRepository> _pedidoRepoMock;
    private readonly Mock<IProductoRepository> _productoRepoMock;
    private readonly Mock<ITipoProductoRepository> _tipoProductoRepoMock;
    private readonly Mock<ILineaRepository> _lineaRepoMock;
    private readonly Mock<IModeloAutobusRepository> _modeloRepoMock;
    private readonly Mock<ILogger<PedidoService>> _loggerMock;
    private readonly PedidoService _pedidoService;

    public PedidoServiceTests()
    {
        _pedidoRepoMock = new Mock<IPedidoRepository>();
        _productoRepoMock = new Mock<IProductoRepository>();
        _tipoProductoRepoMock = new Mock<ITipoProductoRepository>();
        _lineaRepoMock = new Mock<ILineaRepository>();
        _modeloRepoMock = new Mock<IModeloAutobusRepository>();
        _loggerMock = new Mock<ILogger<PedidoService>>();

        _pedidoService = new PedidoService(
            _pedidoRepoMock.Object,
            _productoRepoMock.Object,
            _tipoProductoRepoMock.Object,
            _lineaRepoMock.Object,
            _modeloRepoMock.Object,
            _loggerMock.Object);
    }

    private static Producto CrearProductoActivo(int id = 1, int? tipoProductoId = 1)
    {
        return new Producto
        {
            Id = id,
            Nombre = "Producto Test",
            Precio = 100,
            Activo = true,
            TipoProductoId = tipoProductoId
        };
    }

    private static TipoProducto CrearTipoProducto(int id = 1, bool permiteNombre = true,
        bool permiteNumeroEconomico = true, bool permiteRuta = true, int maxCaracteres = 25)
    {
        return new TipoProducto
        {
            Id = id,
            Nombre = "Tipo Test",
            PermiteNombre = permiteNombre,
            PermiteNumeroEconomico = permiteNumeroEconomico,
            PermiteRuta = permiteRuta,
            MaxCaracteres = maxCaracteres
        };
    }

    private static CarritoItemDto CrearItemValido(int productoId = 1, int modeloId = 1, int lineaId = 1)
    {
        return new CarritoItemDto
        {
            ProductoId = productoId,
            ModeloAutobusId = modeloId,
            LineaId = lineaId,
            Cantidad = 1
        };
    }

    private void SetupReferenciasValidas()
    {
        _productoRepoMock.Setup(r => r.ObtenerPorIdAsync(It.IsAny<int>()))
            .ReturnsAsync(CrearProductoActivo());
        _modeloRepoMock.Setup(r => r.ObtenerPorIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new ModeloAutobus { Id = 1, Nombre = "Irizar i8", Fabricante = "Irizar", Activo = true });
        _lineaRepoMock.Setup(r => r.ObtenerPorIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Linea { Id = 1, Nombre = "ETN", Activa = true });
        _tipoProductoRepoMock.Setup(r => r.ObtenerPorIdAsync(It.IsAny<int>()))
            .ReturnsAsync(CrearTipoProducto());
        _pedidoRepoMock.Setup(r => r.AgregarAsync(It.IsAny<Pedido>()))
            .Callback<Pedido>(p => p.Id = 42);
    }

    // ============ RN-02: Modelo y Línea Obligatorios ============

    [Fact]
    public async Task AgregarAsync_SinModeloAutobus_LanzaValidationException()
    {
        var dto = CrearItemValido();
        dto.ModeloAutobusId = null;

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _pedidoService.AgregarAsync(dto));
        Assert.Contains("modelo", ex.Message.ToLower());
    }

    [Fact]
    public async Task AgregarAsync_SinLinea_LanzaValidationException()
    {
        var dto = CrearItemValido();
        dto.LineaId = null;

        _productoRepoMock.Setup(r => r.ObtenerPorIdAsync(dto.ProductoId))
            .ReturnsAsync(CrearProductoActivo());
        _modeloRepoMock.Setup(r => r.ObtenerPorIdAsync(dto.ModeloAutobusId!.Value))
            .ReturnsAsync(new ModeloAutobus { Id = 1, Nombre = "Irizar i8", Activo = true });

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _pedidoService.AgregarAsync(dto));
        Assert.Contains("línea", ex.Message.ToLower());
    }

    // ============ Cantidad ============

    [Fact]
    public async Task AgregarAsync_CantidadCero_LanzaValidationException()
    {
        var dto = CrearItemValido();
        dto.Cantidad = 0;

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _pedidoService.AgregarAsync(dto));
        Assert.Contains("cantidad", ex.Message.ToLower());
    }

    [Fact]
    public async Task AgregarAsync_CantidadNegativa_LanzaValidationException()
    {
        var dto = CrearItemValido();
        dto.Cantidad = -1;

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _pedidoService.AgregarAsync(dto));
        Assert.Contains("cantidad", ex.Message.ToLower());
    }

    // ============ Producto no existe ============

    [Fact]
    public async Task AgregarAsync_ProductoNoExiste_LanzaValidationException()
    {
        var dto = CrearItemValido();
        _productoRepoMock.Setup(r => r.ObtenerPorIdAsync(dto.ProductoId))
            .ReturnsAsync((Producto?)null);

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _pedidoService.AgregarAsync(dto));
        Assert.Contains("producto", ex.Message.ToLower());
    }

    [Fact]
    public async Task AgregarAsync_ProductoInactivo_LanzaValidationException()
    {
        var dto = CrearItemValido();
        var productoInactivo = CrearProductoActivo();
        productoInactivo.Activo = false;
        _productoRepoMock.Setup(r => r.ObtenerPorIdAsync(dto.ProductoId))
            .ReturnsAsync(productoInactivo);

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _pedidoService.AgregarAsync(dto));
        Assert.Contains("producto", ex.Message.ToLower());
    }

    // ============ Modelo no existe ============

    [Fact]
    public async Task AgregarAsync_ModeloNoExiste_LanzaValidationException()
    {
        var dto = CrearItemValido();
        _productoRepoMock.Setup(r => r.ObtenerPorIdAsync(dto.ProductoId))
            .ReturnsAsync(CrearProductoActivo());
        _modeloRepoMock.Setup(r => r.ObtenerPorIdAsync(dto.ModeloAutobusId!.Value))
            .ReturnsAsync((ModeloAutobus?)null);

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _pedidoService.AgregarAsync(dto));
        Assert.Contains("modelo", ex.Message.ToLower());
    }

    // ============ Línea no existe ============

    [Fact]
    public async Task AgregarAsync_LineaNoExiste_LanzaValidationException()
    {
        var dto = CrearItemValido();
        _productoRepoMock.Setup(r => r.ObtenerPorIdAsync(dto.ProductoId))
            .ReturnsAsync(CrearProductoActivo());
        _modeloRepoMock.Setup(r => r.ObtenerPorIdAsync(dto.ModeloAutobusId!.Value))
            .ReturnsAsync(new ModeloAutobus { Id = 1, Nombre = "Irizar i8", Activo = true });
        _lineaRepoMock.Setup(r => r.ObtenerPorIdAsync(dto.LineaId!.Value))
            .ReturnsAsync((Linea?)null);

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _pedidoService.AgregarAsync(dto));
        Assert.Contains("línea", ex.Message.ToLower());
    }

    // ============ RN-03: Límites de Personalización ============

    [Fact]
    public async Task AgregarAsync_NombreOperadorExcedeMaxCaracteres_LanzaValidationException()
    {
        var dto = CrearItemValido();
        dto.NombreOperador = new string('A', 26); // MaxCaracteres es 25

        SetupReferenciasValidas();

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _pedidoService.AgregarAsync(dto));
        Assert.Contains("nombre", ex.Message.ToLower());
    }

    [Fact]
    public async Task AgregarAsync_RutaExcedeMaxCaracteres_LanzaValidationException()
    {
        var dto = CrearItemValido();
        dto.Ruta = new string('X', 26);

        SetupReferenciasValidas();

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _pedidoService.AgregarAsync(dto));
        Assert.Contains("ruta", ex.Message.ToLower());
    }

    [Fact]
    public async Task AgregarAsync_NombreOperadorDentroDelLimite_Exitoso()
    {
        var dto = CrearItemValido();
        dto.NombreOperador = new string('A', 25); // Justo en el límite

        SetupReferenciasValidas();

        var resultado = await _pedidoService.AgregarAsync(dto);
        Assert.Equal(42, resultado.PedidoId);
        Assert.Contains("agregado", resultado.Mensaje.ToLower());
    }

    // ============ RN-04: Caracteres Permitidos ============

    [Theory]
    [InlineData("Juan<xss>")]
    [InlineData("Name;drop")]
    [InlineData("test@name")]
    public async Task AgregarAsync_CaracteresNoPermitidos_LanzaValidationException(string nombre)
    {
        var dto = CrearItemValido();
        dto.NombreOperador = nombre;

        SetupReferenciasValidas();

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _pedidoService.AgregarAsync(dto));
        Assert.Contains("caracteres no permitidos", ex.Message.ToLower());
    }

    [Theory]
    [InlineData("Juan Pérez")]
    [InlineData("José García-López")]
    [InlineData("México - Guadalajara")]
    [InlineData("Expreso Futura")]
    [InlineData("")]
    public async Task AgregarAsync_CaracteresPermitidos_Exitoso(string nombre)
    {
        var dto = CrearItemValido();
        dto.NombreOperador = nombre;

        SetupReferenciasValidas();

        var resultado = await _pedidoService.AgregarAsync(dto);
        Assert.True(resultado.PedidoId > 0);
    }

    // ============ RN-05: Integridad del Texto ============

    [Fact]
    public async Task AgregarAsync_TextoCompletoSeGuardaSinTruncamiento()
    {
        var dto = CrearItemValido();
        dto.NombreOperador = "Expreso Futura";
        dto.NumeroEconomico = "105";
        dto.Ruta = "México - Guadalajara";

        Pedido? pedidoGuardado = null;
        SetupReferenciasValidas();
        _pedidoRepoMock.Setup(r => r.AgregarAsync(It.IsAny<Pedido>()))
            .Callback<Pedido>(p => pedidoGuardado = p);

        await _pedidoService.AgregarAsync(dto);

        Assert.NotNull(pedidoGuardado);
        Assert.Equal("Expreso Futura", pedidoGuardado!.NombreOperador);
        Assert.Equal("105", pedidoGuardado.NumeroEconomico);
        Assert.Equal("México - Guadalajara", pedidoGuardado.Ruta);
    }

    // ============ RN-10: Estado Inicial ============

    [Fact]
    public async Task AgregarAsync_PedidoIniciaConEstadoRecibido()
    {
        var dto = CrearItemValido();
        Pedido? pedidoGuardado = null;
        SetupReferenciasValidas();
        _pedidoRepoMock.Setup(r => r.AgregarAsync(It.IsAny<Pedido>()))
            .Callback<Pedido>(p => pedidoGuardado = p);

        await _pedidoService.AgregarAsync(dto);

        Assert.NotNull(pedidoGuardado);
        Assert.Equal("Recibido", pedidoGuardado!.Estado);
    }

    // ============ RN-11: Flujo de Estado ============

    [Theory]
    [InlineData("Recibido", "En producción", true)]
    [InlineData("En producción", "Enviado", true)]
    [InlineData("Enviado", "Entregado", true)]
    [InlineData("Recibido", "Recibido", true)] // mismo estado es válido
    [InlineData("Recibido", "Enviado", false)] // saltar estado no es válido
    [InlineData("Recibido", "Entregado", false)] // saltar estados no es válido
    [InlineData("Enviado", "Recibido", false)] // retroceder no es válido
    [InlineData("Entregado", "Enviado", false)] // retroceder no es válido
    [InlineData("Entregado", "En producción", false)] // retroceder no es válido
    public async Task ActualizarEstadoAsync_ValidaTransiciones(string estadoActual, string nuevoEstado, bool esValido)
    {
        var pedido = new Pedido
        {
            Id = 1,
            ProductoId = 1,
            Estado = estadoActual,
            Cantidad = 1
        };

        _pedidoRepoMock.Setup(r => r.ObtenerPorIdAsync(1))
            .ReturnsAsync(pedido);

        if (esValido)
        {
            await _pedidoService.ActualizarEstadoAsync(1, nuevoEstado);
            _pedidoRepoMock.Verify(r => r.ActualizarEstadoAsync(1, nuevoEstado), Times.Once);
        }
        else
        {
            var ex = await Assert.ThrowsAsync<ValidationException>(
                () => _pedidoService.ActualizarEstadoAsync(1, nuevoEstado));
            Assert.Contains("transición", ex.Message.ToLower());
        }
    }

    [Fact]
    public async Task ActualizarEstadoAsync_PedidoNoExiste_LanzaValidationException()
    {
        _pedidoRepoMock.Setup(r => r.ObtenerPorIdAsync(999))
            .ReturnsAsync((Pedido?)null);

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _pedidoService.ActualizarEstadoAsync(999, "Enviado"));
        Assert.Contains("no existe", ex.Message.ToLower());
    }

    // ============ ConfirmarCarrito ============

    [Fact]
    public async Task ConfirmarCarritoAsync_CambiaEstadoAPorduccion()
    {
        var items = new List<Pedido>
        {
            new() { Id = 1, Estado = "Recibido", ProductoId = 1 },
            new() { Id = 2, Estado = "Recibido", ProductoId = 2 }
        };
        _pedidoRepoMock.Setup(r => r.ObtenerConProductosAsync())
            .ReturnsAsync(items);

        await _pedidoService.ConfirmarCarritoAsync();

        _pedidoRepoMock.Verify(r => r.ActualizarEstadoAsync(1, "En producción"), Times.Once);
        _pedidoRepoMock.Verify(r => r.ActualizarEstadoAsync(2, "En producción"), Times.Once);
    }

    // ============ ContarItems ============

    [Fact]
    public async Task ContarItemsAsync_RetornaConteoCorrecto()
    {
        _pedidoRepoMock.Setup(r => r.ContarAsync()).ReturnsAsync(5);

        var count = await _pedidoService.ContarItemsAsync();
        Assert.Equal(5, count);
    }

    // ============ EliminarItem ============

    [Fact]
    public async Task EliminarItemAsync_LlamaRepositorio()
    {
        await _pedidoService.EliminarItemAsync(10);
        _pedidoRepoMock.Verify(r => r.EliminarAsync(10), Times.Once);
    }

    // ============ RN-06: Nueva Línea disponible para todos ============
    // (Esta regla se cumple porque el configurador React carga todas las líneas
    // activas sin filtrar por tipo de producto. Se prueba en el API controller.)

    // ============ Validación de campos opcionales cuando TipoProducto no tiene flags ============

    [Fact]
    public async Task AgregarAsync_TipoProductoSinPermitirNombre_NoValidaLimite()
    {
        var dto = CrearItemValido();
        dto.NombreOperador = new string('X', 100); // largo, pero no se valida porque PermiteNombre = false

        SetupReferenciasValidas();
        _tipoProductoRepoMock.Setup(r => r.ObtenerPorIdAsync(It.IsAny<int>()))
            .ReturnsAsync(CrearTipoProducto(permiteNombre: false, permiteRuta: false));

        var resultado = await _pedidoService.AgregarAsync(dto);
        Assert.True(resultado.PedidoId > 0);
    }

    [Fact]
    public async Task AgregarAsync_SinTipoProductoAsociado_NoValidaLimitesDeTexto()
    {
        var dto = CrearItemValido();
        var producto = CrearProductoActivo(tipoProductoId: null);

        _productoRepoMock.Setup(r => r.ObtenerPorIdAsync(dto.ProductoId))
            .ReturnsAsync(producto);
        _modeloRepoMock.Setup(r => r.ObtenerPorIdAsync(dto.ModeloAutobusId!.Value))
            .ReturnsAsync(new ModeloAutobus { Id = 1, Nombre = "Irizar i8", Activo = true });
        _lineaRepoMock.Setup(r => r.ObtenerPorIdAsync(dto.LineaId!.Value))
            .ReturnsAsync(new Linea { Id = 1, Nombre = "ETN", Activa = true });
        _pedidoRepoMock.Setup(r => r.AgregarAsync(It.IsAny<Pedido>()))
            .Callback<Pedido>(p => p.Id = 42);

        var resultado = await _pedidoService.AgregarAsync(dto);
        Assert.True(resultado.PedidoId > 0);
    }

    // ============ Campos vacíos se convierten a string vacío ============

    [Fact]
    public async Task AgregarAsync_CamposNulosSeConviertenAVacio()
    {
        var dto = CrearItemValido();
        dto.NombreOperador = null;
        dto.NumeroEconomico = null;
        dto.Ruta = null;
        dto.NotasEspeciales = null;

        Pedido? pedidoGuardado = null;
        SetupReferenciasValidas();
        _pedidoRepoMock.Setup(r => r.AgregarAsync(It.IsAny<Pedido>()))
            .Callback<Pedido>(p => pedidoGuardado = p);

        await _pedidoService.AgregarAsync(dto);

        Assert.NotNull(pedidoGuardado);
        Assert.Equal("", pedidoGuardado!.NombreOperador);
        Assert.Equal("", pedidoGuardado.NumeroEconomico);
        Assert.Equal("", pedidoGuardado.Ruta);
        Assert.Equal("", pedidoGuardado.NotasEspeciales);
    }

    // ============ ObtenerCarrito ============

    [Fact]
    public async Task ObtenerCarritoAsync_LlamaRepositorioConProductos()
    {
        var expected = new List<Pedido>();
        _pedidoRepoMock.Setup(r => r.ObtenerConProductosAsync()).ReturnsAsync(expected);

        var result = await _pedidoService.ObtenerCarritoAsync();
        Assert.Same(expected, result);
    }

    // ============ ObtenerTodos ============

    [Fact]
    public async Task ObtenerTodosAsync_LlamaRepositorio()
    {
        var expected = new List<Pedido> { new() { Id = 1 } };
        _pedidoRepoMock.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(expected);

        var result = await _pedidoService.ObtenerTodosAsync();
        Assert.Same(expected, result);
    }

    // ============ ObtenerPorId ============

    [Fact]
    public async Task ObtenerPorIdAsync_LlamaRepositorio()
    {
        var expected = new Pedido { Id = 5 };
        _pedidoRepoMock.Setup(r => r.ObtenerPorIdAsync(5)).ReturnsAsync(expected);

        var result = await _pedidoService.ObtenerPorIdAsync(5);
        Assert.Same(expected, result);
    }
}
