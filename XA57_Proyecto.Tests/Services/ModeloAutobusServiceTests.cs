using Moq;
using XA57_Proyecto.Application.Exceptions;
using XA57_Proyecto.Application.Services;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Infrastructure.Repositories.Interfaces;

namespace XA57_Proyecto.Tests.Services;

public class ModeloAutobusServiceTests
{
    private readonly Mock<IModeloAutobusRepository> _repoMock;
    private readonly ModeloAutobusService _service;

    public ModeloAutobusServiceTests()
    {
        _repoMock = new Mock<IModeloAutobusRepository>();
        _service = new ModeloAutobusService(_repoMock.Object);
    }

    // ============ AgregarAsync ============

    [Fact]
    public async Task AgregarAsync_NombreVacio_LanzaValidationException()
    {
        var modelo = new ModeloAutobus { Nombre = "", Fabricante = "Volvo" };

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _service.AgregarAsync(modelo));
        Assert.Contains("nombre", ex.Message.ToLower());
    }

    [Fact]
    public async Task AgregarAsync_FabricanteVacio_LanzaValidationException()
    {
        var modelo = new ModeloAutobus { Nombre = "Irizar i8", Fabricante = "" };

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _service.AgregarAsync(modelo));
        Assert.Contains("fabricante", ex.Message.ToLower());
    }

    [Fact]
    public async Task AgregarAsync_ModeloValido_LlamaRepository()
    {
        var modelo = new ModeloAutobus { Nombre = "Irizar i8", Fabricante = "Irizar" };
        _repoMock.Setup(r => r.AgregarAsync(modelo)).ReturnsAsync(modelo);

        var result = await _service.AgregarAsync(modelo);
        Assert.NotNull(result);
        _repoMock.Verify(r => r.AgregarAsync(modelo), Times.Once);
    }

    // ============ ActualizarAsync ============

    [Fact]
    public async Task ActualizarAsync_ModeloNoExiste_LanzaValidationException()
    {
        var modelo = new ModeloAutobus { Id = 999, Nombre = "X", Fabricante = "Y" };
        _repoMock.Setup(r => r.ObtenerPorIdAsync(999)).ReturnsAsync((ModeloAutobus?)null);

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _service.ActualizarAsync(modelo));
        Assert.Contains("no existe", ex.Message.ToLower());
    }

    [Fact]
    public async Task ActualizarAsync_NombreVacio_LanzaValidationException()
    {
        var existente = new ModeloAutobus { Id = 1, Nombre = "Old", Fabricante = "F" };
        _repoMock.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(existente);

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _service.ActualizarAsync(new ModeloAutobus { Id = 1, Nombre = "", Fabricante = "Y" }));
        Assert.Contains("nombre", ex.Message.ToLower());
    }

    [Fact]
    public async Task ActualizarAsync_FabricanteVacio_LanzaValidationException()
    {
        var existente = new ModeloAutobus { Id = 1, Nombre = "Old", Fabricante = "F" };
        _repoMock.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(existente);

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _service.ActualizarAsync(new ModeloAutobus { Id = 1, Nombre = "New", Fabricante = "" }));
        Assert.Contains("fabricante", ex.Message.ToLower());
    }

    [Fact]
    public async Task ActualizarAsync_Valido_LlamaRepository()
    {
        var existente = new ModeloAutobus { Id = 1, Nombre = "Old", Fabricante = "OldF" };
        var actualizado = new ModeloAutobus { Id = 1, Nombre = "New", Fabricante = "NewF" };
        _repoMock.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(existente);

        await _service.ActualizarAsync(actualizado);
        _repoMock.Verify(r => r.ActualizarAsync(actualizado), Times.Once);
    }

    // ============ EliminarAsync ============

    [Fact]
    public async Task EliminarAsync_ModeloNoExiste_LanzaValidationException()
    {
        _repoMock.Setup(r => r.ObtenerPorIdAsync(999)).ReturnsAsync((ModeloAutobus?)null);

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _service.EliminarAsync(999));
        Assert.Contains("no existe", ex.Message.ToLower());
    }

    [Fact]
    public async Task EliminarAsync_ModeloExiste_SoftDelete()
    {
        _repoMock.Setup(r => r.ObtenerPorIdAsync(1))
            .ReturnsAsync(new ModeloAutobus { Id = 1, Nombre = "Irizar i8", Fabricante = "Irizar", Activo = true });

        await _service.EliminarAsync(1);
        _repoMock.Verify(r => r.EliminarAsync(1), Times.Once);
    }

    // ============ Consultas ============

    [Fact]
    public async Task ObtenerTodosAsync_RetornaTodos()
    {
        var expected = new List<ModeloAutobus> { new() { Id = 1 } };
        _repoMock.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(expected);

        var result = await _service.ObtenerTodosAsync();
        Assert.Same(expected, result);
    }

    [Fact]
    public async Task ObtenerActivosAsync_RetornaActivos()
    {
        var expected = new List<ModeloAutobus> { new() { Id = 1, Activo = true } };
        _repoMock.Setup(r => r.ObtenerActivosAsync()).ReturnsAsync(expected);

        var result = await _service.ObtenerActivosAsync();
        Assert.Same(expected, result);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_RetornaModelo()
    {
        var expected = new ModeloAutobus { Id = 5 };
        _repoMock.Setup(r => r.ObtenerPorIdAsync(5)).ReturnsAsync(expected);

        var result = await _service.ObtenerPorIdAsync(5);
        Assert.Same(expected, result);
    }
}
