using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Moq;
using XA57_Proyecto.Application.DTOs;
using XA57_Proyecto.Application.Exceptions;
using XA57_Proyecto.Application.Interfaces;
using XA57_Proyecto.Controllers;
using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Tests.Controllers;

public class CarritoControllerTests
{
    private readonly Mock<IPedidoService> _pedidoServiceMock;
    private readonly CarritoController _controller;

    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public CarritoControllerTests()
    {
        _pedidoServiceMock = new Mock<IPedidoService>();
        _controller = new CarritoController(_pedidoServiceMock.Object);
    }

    private static T? GetValue<T>(IActionResult result)
    {
        var json = JsonSerializer.Serialize(((ObjectResult)result).Value);
        return JsonSerializer.Deserialize<T>(json, JsonOptions);
    }

    // ============ Index ============

    [Fact]
    public async Task Index_RetornaVistaConItems()
    {
        var items = new List<Pedido>
        {
            new() { Id = 1, ProductoId = 1, Cantidad = 2, Estado = "Recibido" }
        };
        _pedidoServiceMock.Setup(s => s.ObtenerCarritoAsync()).ReturnsAsync(items);

        var result = await _controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ViewModels.CarritoViewModel>(viewResult.Model);
        Assert.Single(model.Items);
    }

    // ============ Agregar ============

    [Fact]
    public async Task Agregar_ItemValido_RetornaOk()
    {
        var dto = new CarritoItemDto { ProductoId = 1, ModeloAutobusId = 1, LineaId = 1, Cantidad = 1 };
        _pedidoServiceMock.Setup(s => s.AgregarAsync(dto))
            .ReturnsAsync(new PedidoResultDto { Mensaje = "Producto agregado al carrito.", PedidoId = 42 });

        var result = await _controller.Agregar(dto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var json = JsonSerializer.Serialize(okResult.Value);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;
        Assert.Equal("Producto agregado al carrito.", root.GetProperty("mensaje").GetString());
        Assert.Equal(42, root.GetProperty("pedidoId").GetInt32());
    }

    [Fact]
    public async Task Agregar_ItemNulo_RetornaBadRequest()
    {
        var result = await _controller.Agregar(null!);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        var json = JsonSerializer.Serialize(badRequest.Value);
        using var doc = JsonDocument.Parse(json);
        Assert.Contains("inválidos", doc.RootElement.GetProperty("mensaje").GetString());
    }

    [Fact]
    public async Task Agregar_ProductoIdCero_RetornaBadRequest()
    {
        var dto = new CarritoItemDto { ProductoId = 0 };

        var result = await _controller.Agregar(dto);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Agregar_ValidationException_RetornaBadRequest()
    {
        var dto = new CarritoItemDto { ProductoId = 1, Cantidad = -1 };
        _pedidoServiceMock.Setup(s => s.AgregarAsync(dto))
            .ThrowsAsync(new ValidationException("La cantidad debe ser mayor a cero."));

        var result = await _controller.Agregar(dto);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        var json = JsonSerializer.Serialize(badRequest.Value);
        using var doc = JsonDocument.Parse(json);
        Assert.Contains("cantidad", doc.RootElement.GetProperty("mensaje").GetString());
    }

    // ============ Eliminar ============

    [Fact]
    public async Task Eliminar_RedirigeAIndex()
    {
        var result = await _controller.Eliminar(1);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);
        _pedidoServiceMock.Verify(s => s.EliminarItemAsync(1), Times.Once);
    }

    // ============ Confirmar ============

    [Fact]
    public async Task Confirmar_LlamaServicioYRedirige()
    {
        var result = await _controller.Confirmar();

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);
        _pedidoServiceMock.Verify(s => s.ConfirmarCarritoAsync(), Times.Once);
    }

    // ============ Cantidad ============

    [Fact]
    public async Task Cantidad_RetornaEntero()
    {
        _pedidoServiceMock.Setup(s => s.ContarItemsAsync()).ReturnsAsync(5);

        var result = await _controller.Cantidad();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(5, okResult.Value);
    }
}
