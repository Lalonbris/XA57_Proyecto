using Moq;
using XA57_Proyecto.Application.Exceptions;
using XA57_Proyecto.Application.Services;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Infrastructure.Repositories.Interfaces;

namespace XA57_Proyecto.Tests.Services;

public class ProductoServiceTests
{
    private readonly Mock<IProductoRepository> _productoRepoMock;
    private readonly Mock<ITipoProductoRepository> _tipoProductoRepoMock;
    private readonly Mock<ILineaRepository> _lineaRepoMock;
    private readonly ProductoService _productoService;

    public ProductoServiceTests()
    {
        _productoRepoMock = new Mock<IProductoRepository>();
        _tipoProductoRepoMock = new Mock<ITipoProductoRepository>();
        _lineaRepoMock = new Mock<ILineaRepository>();

        _productoService = new ProductoService(
            _productoRepoMock.Object,
            _tipoProductoRepoMock.Object,
            _lineaRepoMock.Object);
    }

    // ============ AgregarAsync ============

    [Fact]
    public async Task AgregarAsync_NombreVacio_LanzaValidationException()
    {
        var producto = new Producto { Nombre = "", Precio = 100 };

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _productoService.AgregarAsync(producto));
        Assert.Contains("nombre", ex.Message.ToLower());
    }

    [Fact]
    public async Task AgregarAsync_NombreSoloEspacios_LanzaValidationException()
    {
        var producto = new Producto { Nombre = "   ", Precio = 100 };

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _productoService.AgregarAsync(producto));
        Assert.Contains("nombre", ex.Message.ToLower());
    }

    [Fact]
    public async Task AgregarAsync_NombreExcede100Caracteres_LanzaValidationException()
    {
        var producto = new Producto { Nombre = new string('A', 101), Precio = 100 };

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _productoService.AgregarAsync(producto));
        Assert.Contains("nombre", ex.Message.ToLower());
    }

    [Fact]
    public async Task AgregarAsync_Nombre100Caracteres_Exitoso()
    {
        var producto = new Producto { Nombre = new string('A', 100), Precio = 100 };
        _productoRepoMock.Setup(r => r.AgregarAsync(producto)).ReturnsAsync(producto);

        var result = await _productoService.AgregarAsync(producto);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task AgregarAsync_PrecioCero_LanzaValidationException()
    {
        var producto = new Producto { Nombre = "Producto", Precio = 0 };

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _productoService.AgregarAsync(producto));
        Assert.Contains("precio", ex.Message.ToLower());
    }

    [Fact]
    public async Task AgregarAsync_PrecioNegativo_LanzaValidationException()
    {
        var producto = new Producto { Nombre = "Producto", Precio = -10 };

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _productoService.AgregarAsync(producto));
        Assert.Contains("precio", ex.Message.ToLower());
    }

    [Fact]
    public async Task AgregarAsync_TipoProductoInvalido_LanzaValidationException()
    {
        var producto = new Producto { Nombre = "Producto", Precio = 100, TipoProductoId = 999 };
        _tipoProductoRepoMock.Setup(r => r.ObtenerPorIdAsync(999))
            .ReturnsAsync((TipoProducto?)null);

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _productoService.AgregarAsync(producto));
        Assert.Contains("tipo de producto", ex.Message.ToLower());
    }

    [Fact]
    public async Task AgregarAsync_TipoProductoValido_Exitoso()
    {
        var producto = new Producto { Nombre = "Producto", Precio = 100, TipoProductoId = 1 };
        _tipoProductoRepoMock.Setup(r => r.ObtenerPorIdAsync(1))
            .ReturnsAsync(new TipoProducto { Id = 1, Nombre = "Tipo Test" });
        _productoRepoMock.Setup(r => r.AgregarAsync(producto)).ReturnsAsync(producto);

        var result = await _productoService.AgregarAsync(producto);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task AgregarAsync_LineaInvalida_LanzaValidationException()
    {
        var producto = new Producto { Nombre = "Producto", Precio = 100, LineaId = 999 };
        _lineaRepoMock.Setup(r => r.ObtenerPorIdAsync(999))
            .ReturnsAsync((Linea?)null);

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _productoService.AgregarAsync(producto));
        Assert.Contains("línea", ex.Message.ToLower());
    }

    [Fact]
    public async Task AgregarAsync_ProductoValido_LlamaRepository()
    {
        var producto = new Producto { Nombre = "Producto", Precio = 150.50m };
        _productoRepoMock.Setup(r => r.AgregarAsync(producto)).ReturnsAsync(producto);

        var result = await _productoService.AgregarAsync(producto);
        _productoRepoMock.Verify(r => r.AgregarAsync(producto), Times.Once);
    }

    // ============ ActualizarAsync ============

    [Fact]
    public async Task ActualizarAsync_ProductoNoExiste_LanzaValidationException()
    {
        var producto = new Producto { Id = 999, Nombre = "X", Precio = 100 };
        _productoRepoMock.Setup(r => r.ObtenerPorIdAsync(999))
            .ReturnsAsync((Producto?)null);

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _productoService.ActualizarAsync(producto));
        Assert.Contains("no existe", ex.Message.ToLower());
    }

    [Fact]
    public async Task ActualizarAsync_ProductoExiste_Exitoso()
    {
        var existente = new Producto { Id = 1, Nombre = "Old", Precio = 50 };
        var actualizado = new Producto { Id = 1, Nombre = "New", Precio = 100 };
        _productoRepoMock.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(existente);

        await _productoService.ActualizarAsync(actualizado);
        _productoRepoMock.Verify(r => r.ActualizarAsync(actualizado), Times.Once);
    }

    [Fact]
    public async Task ActualizarAsync_ValidaNombreVacio()
    {
        var existente = new Producto { Id = 1, Nombre = "Old", Precio = 50 };
        _productoRepoMock.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(existente);

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _productoService.ActualizarAsync(new Producto { Id = 1, Nombre = "", Precio = 100 }));
        Assert.Contains("nombre", ex.Message.ToLower());
    }

    // ============ EliminarAsync ============

    [Fact]
    public async Task EliminarAsync_ProductoNoExiste_LanzaValidationException()
    {
        _productoRepoMock.Setup(r => r.ObtenerPorIdAsync(999))
            .ReturnsAsync((Producto?)null);

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _productoService.EliminarAsync(999));
        Assert.Contains("no existe", ex.Message.ToLower());
    }

    [Fact]
    public async Task EliminarAsync_ProductoExiste_EliminaSoftDelete()
    {
        _productoRepoMock.Setup(r => r.ObtenerPorIdAsync(1))
            .ReturnsAsync(new Producto { Id = 1, Nombre = "Prod", Precio = 100 });

        await _productoService.EliminarAsync(1);
        _productoRepoMock.Verify(r => r.EliminarAsync(1), Times.Once);
    }

    // ============ Consultas ============

    [Fact]
    public async Task ObtenerTodosAsync_RetornaLosProductos()
    {
        var expected = new List<Producto> { new() { Id = 1, Nombre = "P1" } };
        _productoRepoMock.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(expected);

        var result = await _productoService.ObtenerTodosAsync();
        Assert.Same(expected, result);
    }

    [Fact]
    public async Task ObtenerTodosAdminAsync_RetornaTodos()
    {
        var expected = new List<Producto> { new() { Id = 1 } };
        _productoRepoMock.Setup(r => r.ObtenerTodosAdminAsync()).ReturnsAsync(expected);

        var result = await _productoService.ObtenerTodosAdminAsync();
        Assert.Same(expected, result);
    }

    [Fact]
    public async Task ObtenerPorTipoAsync_FiltraCorrectamente()
    {
        var expected = new List<Producto> { new() { Id = 1 } };
        _productoRepoMock.Setup(r => r.ObtenerPorTipoAsync(1)).ReturnsAsync(expected);

        var result = await _productoService.ObtenerPorTipoAsync(1);
        Assert.Same(expected, result);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_RetornaProducto()
    {
        var expected = new Producto { Id = 5 };
        _productoRepoMock.Setup(r => r.ObtenerPorIdAsync(5)).ReturnsAsync(expected);

        var result = await _productoService.ObtenerPorIdAsync(5);
        Assert.Same(expected, result);
    }
}
