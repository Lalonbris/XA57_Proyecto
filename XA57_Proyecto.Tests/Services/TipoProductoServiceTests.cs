using Moq;
using XA57_Proyecto.Application.Services;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Infrastructure.Repositories.Interfaces;

namespace XA57_Proyecto.Tests.Services;

public class TipoProductoServiceTests
{
    private readonly Mock<ITipoProductoRepository> _repoMock;
    private readonly TipoProductoService _service;

    public TipoProductoServiceTests()
    {
        _repoMock = new Mock<ITipoProductoRepository>();
        _service = new TipoProductoService(_repoMock.Object);
    }

    [Fact]
    public async Task ObtenerTodosAsync_LlamaRepository()
    {
        var expected = new List<TipoProducto> { new() { Id = 1, Nombre = "Busito de Peluche" } };
        _repoMock.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(expected);

        var result = await _service.ObtenerTodosAsync();
        Assert.Same(expected, result);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_LlamaRepository()
    {
        var expected = new TipoProducto { Id = 1, Nombre = "Busito", MaxCaracteres = 25 };
        _repoMock.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(expected);

        var result = await _service.ObtenerPorIdAsync(1);
        Assert.Same(expected, result);
    }

    [Fact]
    public async Task AgregarAsync_LlamaRepository()
    {
        var tipo = new TipoProducto { Nombre = "Taza", MaxCaracteres = 20, PermiteNombre = true };
        _repoMock.Setup(r => r.AgregarAsync(tipo)).ReturnsAsync(tipo);

        var result = await _service.AgregarAsync(tipo);
        Assert.NotNull(result);
        _repoMock.Verify(r => r.AgregarAsync(tipo), Times.Once);
    }

    [Fact]
    public async Task ActualizarAsync_LlamaRepository()
    {
        var tipo = new TipoProducto { Id = 1, Nombre = "Actualizado" };
        await _service.ActualizarAsync(tipo);
        _repoMock.Verify(r => r.ActualizarAsync(tipo), Times.Once);
    }

    [Fact]
    public async Task EliminarAsync_LlamaRepository()
    {
        await _service.EliminarAsync(3);
        _repoMock.Verify(r => r.EliminarAsync(3), Times.Once);
    }
}
