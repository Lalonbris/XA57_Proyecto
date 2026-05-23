using Moq;
using XA57_Proyecto.Application.Services;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Infrastructure.Repositories.Interfaces;

namespace XA57_Proyecto.Tests.Services;

public class LineaServiceTests
{
    private readonly Mock<ILineaRepository> _repoMock;
    private readonly LineaService _service;

    public LineaServiceTests()
    {
        _repoMock = new Mock<ILineaRepository>();
        _service = new LineaService(_repoMock.Object);
    }

    [Fact]
    public async Task ObtenerTodosAsync_LlamaRepository()
    {
        var expected = new List<Linea> { new() { Id = 1, Nombre = "ETN" } };
        _repoMock.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(expected);

        var result = await _service.ObtenerTodosAsync();
        Assert.Same(expected, result);
    }

    [Fact]
    public async Task ObtenerActivasAsync_LlamaRepository()
    {
        var expected = new List<Linea> { new() { Id = 1, Nombre = "ETN", Activa = true } };
        _repoMock.Setup(r => r.ObtenerActivasAsync()).ReturnsAsync(expected);

        var result = await _service.ObtenerActivasAsync();
        Assert.Same(expected, result);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_LlamaRepository()
    {
        var expected = new Linea { Id = 3, Nombre = "Omnibus de México" };
        _repoMock.Setup(r => r.ObtenerPorIdAsync(3)).ReturnsAsync(expected);

        var result = await _service.ObtenerPorIdAsync(3);
        Assert.Same(expected, result);
    }

    [Fact]
    public async Task AgregarAsync_LlamaRepository()
    {
        var linea = new Linea { Nombre = "Nueva Linea", ColorPrimario = "#FF0000" };
        _repoMock.Setup(r => r.AgregarAsync(linea)).ReturnsAsync(linea);

        var result = await _service.AgregarAsync(linea);
        Assert.NotNull(result);
        _repoMock.Verify(r => r.AgregarAsync(linea), Times.Once);
    }

    [Fact]
    public async Task ActualizarAsync_LlamaRepository()
    {
        var linea = new Linea { Id = 1, Nombre = "Actualizada" };

        await _service.ActualizarAsync(linea);
        _repoMock.Verify(r => r.ActualizarAsync(linea), Times.Once);
    }

    [Fact]
    public async Task EliminarAsync_LlamaRepository()
    {
        await _service.EliminarAsync(5);
        _repoMock.Verify(r => r.EliminarAsync(5), Times.Once);
    }
}
