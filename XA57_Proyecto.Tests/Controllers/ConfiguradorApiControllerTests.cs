using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Moq;
using XA57_Proyecto.Application.Interfaces;
using XA57_Proyecto.Controllers;
using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Tests.Controllers;

public class ConfiguradorApiControllerTests
{
    private readonly Mock<IModeloAutobusService> _modeloServiceMock;
    private readonly Mock<ILineaService> _lineaServiceMock;
    private readonly Mock<IProductoService> _productoServiceMock;
    private readonly ConfiguradorApiController _controller;

    public ConfiguradorApiControllerTests()
    {
        _modeloServiceMock = new Mock<IModeloAutobusService>();
        _lineaServiceMock = new Mock<ILineaService>();
        _productoServiceMock = new Mock<IProductoService>();
        _controller = new ConfiguradorApiController(
            _modeloServiceMock.Object,
            _lineaServiceMock.Object,
            _productoServiceMock.Object);
    }

    // ============ GetModelos ============

    [Fact]
    public async Task GetModelos_RetornaListaModelosActivos()
    {
        var modelos = new List<ModeloAutobus>
        {
            new() { Id = 1, Nombre = "Irizar i8", Fabricante = "Irizar", Activo = true },
            new() { Id = 2, Nombre = "Volvo 9800", Fabricante = "Volvo", Activo = true }
        };
        _modeloServiceMock.Setup(s => s.ObtenerActivosAsync()).ReturnsAsync(modelos);

        var result = await _controller.GetModelos();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var json = JsonSerializer.Serialize(okResult.Value);
        using var doc = JsonDocument.Parse(json);
        Assert.Equal(2, doc.RootElement.GetArrayLength());
        Assert.Equal("Irizar i8", doc.RootElement[0].GetProperty("Nombre").GetString());
    }

    [Fact]
    public async Task GetModelos_SinModelos_RetornaListaVacia()
    {
        _modeloServiceMock.Setup(s => s.ObtenerActivosAsync())
            .ReturnsAsync(new List<ModeloAutobus>());

        var result = await _controller.GetModelos();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    // ============ GetLineas ============

    [Fact]
    public async Task GetLineas_RetornaTodasLasLineasActivas()
    {
        var lineas = new List<Linea>
        {
            new() { Id = 1, Nombre = "ETN", ColorPrimario = "#FF0000", ColorSecundario = "#FFF", LogoUrl = "logo.png", Activa = true },
            new() { Id = 2, Nombre = "Omnibus de México", ColorPrimario = "#003087", ColorSecundario = "#C8A800", Activa = true }
        };
        _lineaServiceMock.Setup(s => s.ObtenerActivasAsync()).ReturnsAsync(lineas);

        var result = await _controller.GetLineas();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var json = JsonSerializer.Serialize(okResult.Value);
        using var doc = JsonDocument.Parse(json);
        Assert.Equal(2, doc.RootElement.GetArrayLength());
    }

    // RN-06: Nueva línea disponible automáticamente para todos los productos
    [Fact]
    public async Task GetLineas_RN06_NuevaLineaDisponibleParaTodos()
    {
        var lineas = new List<Linea>
        {
            new() { Id = 1, Nombre = "ETN", Activa = true },
            new() { Id = 2, Nombre = "Estrella Roja", Activa = true },
            new() { Id = 3, Nombre = "Futura", Activa = true }
        };
        _lineaServiceMock.Setup(s => s.ObtenerActivasAsync()).ReturnsAsync(lineas);

        var result = await _controller.GetLineas();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var json = JsonSerializer.Serialize(okResult.Value);
        using var doc = JsonDocument.Parse(json);
        Assert.Equal(3, doc.RootElement.GetArrayLength());
    }

    // ============ GetProducto ============

    [Fact]
    public async Task GetProducto_ProductoExiste_RetornaConTipoProducto()
    {
        var tipoProducto = new TipoProducto
        {
            Id = 1,
            Nombre = "Busito de Peluche",
            MaxCaracteres = 25,
            PermiteNombre = true,
            PermiteNumeroEconomico = true,
            PermiteRuta = true
        };
        var producto = new Producto
        {
            Id = 5,
            Nombre = "Busito Irizar",
            Precio = 350,
            ImagenUrl = "busito.jpg",
            TipoProducto = tipoProducto,
            TipoProductoId = 1
        };
        _productoServiceMock.Setup(s => s.ObtenerPorIdAsync(5)).ReturnsAsync(producto);

        var result = await _controller.GetProducto(5);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var json = JsonSerializer.Serialize(okResult.Value);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;
        Assert.Equal(5, root.GetProperty("Id").GetInt32());
        Assert.Equal("Busito Irizar", root.GetProperty("Nombre").GetString());
        Assert.Equal(350m, root.GetProperty("Precio").GetDecimal());
        var tp = root.GetProperty("tipoProducto");
        Assert.True(tp.GetProperty("PermiteNombre").GetBoolean());
        Assert.True(tp.GetProperty("PermiteNumeroEconomico").GetBoolean());
        Assert.True(tp.GetProperty("PermiteRuta").GetBoolean());
        Assert.Equal(25, tp.GetProperty("MaxCaracteres").GetInt32());
    }

    [Fact]
    public async Task GetProducto_ProductoNoExiste_RetornaNotFound()
    {
        _productoServiceMock.Setup(s => s.ObtenerPorIdAsync(999))
            .ReturnsAsync((Producto?)null);

        var result = await _controller.GetProducto(999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetProducto_SinTipoProducto_RetornaTipoNulo()
    {
        var producto = new Producto
        {
            Id = 1,
            Nombre = "Producto sin tipo",
            Precio = 100,
            TipoProducto = null,
            TipoProductoId = null
        };
        _productoServiceMock.Setup(s => s.ObtenerPorIdAsync(1)).ReturnsAsync(producto);

        var result = await _controller.GetProducto(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var json = JsonSerializer.Serialize(okResult.Value);
        using var doc = JsonDocument.Parse(json);
        Assert.True(doc.RootElement.GetProperty("tipoProducto").ValueKind == JsonValueKind.Null);
    }
}
