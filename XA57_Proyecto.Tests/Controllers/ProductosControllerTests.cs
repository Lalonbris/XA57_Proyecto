using Microsoft.AspNetCore.Mvc;
using Moq;
using XA57_Proyecto.Application.Interfaces;
using XA57_Proyecto.Controllers;
using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Tests.Controllers;

public class ProductosControllerTests
{
    private readonly Mock<IProductoService> _productoServiceMock;
    private readonly ProductosController _controller;

    public ProductosControllerTests()
    {
        _productoServiceMock = new Mock<IProductoService>();
        _controller = new ProductosController(_productoServiceMock.Object);
    }

    [Fact]
    public async Task Detalle_ProductoExiste_RetornaVista()
    {
        var producto = new Producto { Id = 1, Nombre = "Busito", Precio = 350 };
        _productoServiceMock.Setup(s => s.ObtenerPorIdAsync(1)).ReturnsAsync(producto);

        var result = await _controller.Detalle(1);
        var view = Assert.IsType<ViewResult>(result);
        Assert.Same(producto, view.Model);
    }

    [Fact]
    public async Task Detalle_ProductoNoExiste_RetornaNotFound()
    {
        _productoServiceMock.Setup(s => s.ObtenerPorIdAsync(999)).ReturnsAsync((Producto?)null);

        var result = await _controller.Detalle(999);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Catalogo_RetornaVistaConProductos()
    {
        var productos = new List<Producto> { new() { Id = 1 }, new() { Id = 2 } };
        _productoServiceMock.Setup(s => s.ObtenerTodosAsync()).ReturnsAsync(productos);

        var result = await _controller.Catalogo();
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<List<Producto>>(view.Model);
        Assert.Equal(2, model.Count);
    }
}
