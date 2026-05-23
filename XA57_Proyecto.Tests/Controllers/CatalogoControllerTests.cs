using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Moq;
using XA57_Proyecto.Application.Interfaces;
using XA57_Proyecto.Controllers;
using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Tests.Controllers;

public class CatalogoControllerTests
{
    private readonly Mock<ITipoProductoService> _tipoProductoServiceMock;
    private readonly Mock<IProductoService> _productoServiceMock;
    private readonly CatalogoController _controller;

    public CatalogoControllerTests()
    {
        _tipoProductoServiceMock = new Mock<ITipoProductoService>();
        _productoServiceMock = new Mock<IProductoService>();
        _controller = new CatalogoController(_tipoProductoServiceMock.Object, _productoServiceMock.Object);
    }

    [Fact]
    public async Task Index_RetornaVistaConTipos()
    {
        var tipos = new List<TipoProducto> { new() { Id = 1, Nombre = "Busito" } };
        _tipoProductoServiceMock.Setup(s => s.ObtenerTodosAsync()).ReturnsAsync(tipos);

        var result = await _controller.Index();
        var view = Assert.IsType<ViewResult>(result);
        Assert.Same(tipos, view.Model);
    }

    [Fact]
    public async Task PorTipo_RetornaProductosFiltradosYViewBag()
    {
        var productos = new List<Producto> { new() { Id = 1, TipoProductoId = 1 } };
        var tipo = new TipoProducto { Id = 1, Nombre = "Busito" };
        _productoServiceMock.Setup(s => s.ObtenerPorTipoAsync(1)).ReturnsAsync(productos);
        _tipoProductoServiceMock.Setup(s => s.ObtenerPorIdAsync(1)).ReturnsAsync(tipo);

        var result = await _controller.PorTipo(1);
        var view = Assert.IsType<ViewResult>(result);
        Assert.Same(productos, view.Model);
        Assert.Equal(tipo, view.ViewData["TipoProducto"]);
    }

    [Fact]
    public async Task ObtenerTodos_RetornaJsonConTipos()
    {
        var tipos = new List<TipoProducto> { new() { Id = 1, Nombre = "Busito" } };
        _tipoProductoServiceMock.Setup(s => s.ObtenerTodosAsync()).ReturnsAsync(tipos);

        var result = await _controller.ObtenerTodos();
        var json = Assert.IsType<JsonResult>(result);
        Assert.Same(tipos, json.Value);
    }

    [Fact]
    public async Task ObtenerPorId_Existe_RetornaJson()
    {
        var tipo = new TipoProducto { Id = 1, Nombre = "Busito" };
        _tipoProductoServiceMock.Setup(s => s.ObtenerPorIdAsync(1)).ReturnsAsync(tipo);

        var result = await _controller.ObtenerPorId(1);
        var json = Assert.IsType<JsonResult>(result);
        Assert.Same(tipo, json.Value);
    }

    [Fact]
    public async Task Crear_Valido_RetornaOk()
    {
        var tipo = new TipoProducto { Nombre = "Nuevo", MaxCaracteres = 20 };
        _tipoProductoServiceMock.Setup(s => s.AgregarAsync(tipo)).ReturnsAsync(tipo);

        var result = await _controller.Crear(tipo);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Editar_IdNoCoincide_RetornaBadRequest()
    {
        var result = await _controller.Editar(1, new TipoProducto { Id = 2, Nombre = "X" });
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Editar_Valido_RetornaOk()
    {
        var tipo = new TipoProducto { Id = 1, Nombre = "Actualizado" };

        var result = await _controller.Editar(1, tipo);
        Assert.IsType<OkResult>(result);
        _tipoProductoServiceMock.Verify(s => s.ActualizarAsync(tipo), Times.Once);
    }

    [Fact]
    public async Task Eliminar_Valido_RetornaOk()
    {
        var result = await _controller.Eliminar(1);
        Assert.IsType<OkResult>(result);
        _tipoProductoServiceMock.Verify(s => s.EliminarAsync(1), Times.Once);
    }
}
