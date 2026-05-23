using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using XA57_Proyecto.Application.Interfaces;
using XA57_Proyecto.Controllers;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.ViewModels;

namespace XA57_Proyecto.Tests.Controllers;

public class HomeControllerTests
{
    private readonly Mock<ILogger<HomeController>> _loggerMock;
    private readonly Mock<IProductoService> _productoServiceMock;
    private readonly HomeController _controller;

    public HomeControllerTests()
    {
        _loggerMock = new Mock<ILogger<HomeController>>();
        _productoServiceMock = new Mock<IProductoService>();
        _controller = new HomeController(_loggerMock.Object, _productoServiceMock.Object);
    }

    [Fact]
    public async Task Index_RetornaPrimeros4Productos()
    {
        var productos = new List<Producto>
        {
            new() { Id = 5, Nombre = "P5" },
            new() { Id = 4, Nombre = "P4" },
            new() { Id = 3, Nombre = "P3" },
            new() { Id = 2, Nombre = "P2" },
            new() { Id = 1, Nombre = "P1" }
        };
        _productoServiceMock.Setup(s => s.ObtenerTodosAsync()).ReturnsAsync(productos);

        var result = await _controller.Index();
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<List<Producto>>(view.Model);
        Assert.Equal(4, model.Count);
        Assert.Equal(5, model[0].Id); // El más reciente primero
    }

    [Fact]
    public async Task Index_Error_RetornaVistaConListaVacia()
    {
        _productoServiceMock.Setup(s => s.ObtenerTodosAsync())
            .ThrowsAsync(new Exception("DB error"));

        var result = await _controller.Index();
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<List<Producto>>(view.Model);
        Assert.Empty(model);
    }

    [Fact]
    public async Task Catalogo_RetornaTodosLosProductos()
    {
        var productos = new List<Producto> { new() { Id = 1 }, new() { Id = 2 } };
        _productoServiceMock.Setup(s => s.ObtenerTodosAsync()).ReturnsAsync(productos);

        var result = await _controller.Catalogo();
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<List<Producto>>(view.Model);
        Assert.Equal(2, model.Count);
    }

    [Fact]
    public async Task Catalogo_Error_RetornaListaVacia()
    {
        _productoServiceMock.Setup(s => s.ObtenerTodosAsync())
            .ThrowsAsync(new Exception("DB error"));

        var result = await _controller.Catalogo();
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<List<Producto>>(view.Model);
        Assert.Empty(model);
    }

    [Fact]
    public async Task CatalogoPorTipo_TipoValido_RetornaFiltrados()
    {
        var productos = new List<Producto> { new() { Id = 1, TipoProductoId = 1 } };
        _productoServiceMock.Setup(s => s.ObtenerPorTipoAsync(1)).ReturnsAsync(productos);

        var result = await _controller.CatalogoPorTipo(1);
        var view = Assert.IsType<ViewResult>(result);
        Assert.Equal("Catalogo", view.ViewName);
        var model = Assert.IsType<List<Producto>>(view.Model);
        Assert.Single(model);
    }

    [Fact]
    public async Task CatalogoPorTipo_TipoCero_RetornaTodos()
    {
        var productos = new List<Producto> { new() { Id = 1 }, new() { Id = 2 } };
        _productoServiceMock.Setup(s => s.ObtenerTodosAsync()).ReturnsAsync(productos);

        var result = await _controller.CatalogoPorTipo(0);
        var view = Assert.IsType<ViewResult>(result);
        Assert.Equal("Catalogo", view.ViewName);
        var model = Assert.IsType<List<Producto>>(view.Model);
        Assert.Equal(2, model.Count);
    }

    [Fact]
    public void Privacy_RetornaVista()
    {
        var result = _controller.Privacy();
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Error_RetornaErrorViewModel()
    {
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        var result = _controller.Error();
        var view = Assert.IsType<ViewResult>(result);
        Assert.IsType<ErrorViewModel>(view.Model);
    }
}
