using Microsoft.EntityFrameworkCore;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Infrastructure.Data;
using XA57_Proyecto.Infrastructure.Repositories;

namespace XA57_Proyecto.Tests.Repositories;

public class ProductoRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly ProductoRepository _repository;

    public ProductoRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")
            .Options;

        _context = new AppDbContext(options);
        _repository = new ProductoRepository(_context);
    }

    [Fact]
    public async Task ObtenerTodosAsync_RetornaSoloActivos_EIncluyeRelaciones()
    {
        var tipoProducto = new TipoProducto { Id = 1, Nombre = "Busito" };
        var linea = new Linea { Id = 1, Nombre = "ETN", Activa = true };
        _context.TiposProducto.Add(tipoProducto);
        _context.Lineas.Add(linea);

        _context.Productos.Add(new Producto { Id = 1, Nombre = "Activo", Precio = 100, Activo = true, TipoProductoId = 1, LineaId = 1 });
        _context.Productos.Add(new Producto { Id = 2, Nombre = "Inactivo", Precio = 50, Activo = false });
        await _context.SaveChangesAsync();

        var result = await _repository.ObtenerTodosAsync();

        Assert.Single(result);
        Assert.Equal("Activo", result[0].Nombre);
        Assert.NotNull(result[0].TipoProducto);
        Assert.NotNull(result[0].Linea);
    }

    [Fact]
    public async Task ObtenerTodosAdminAsync_RetornaTodos_IncluyendoInactivos()
    {
        _context.Productos.Add(new Producto { Id = 1, Nombre = "Activo", Precio = 100, Activo = true });
        _context.Productos.Add(new Producto { Id = 2, Nombre = "Inactivo", Precio = 50, Activo = false });
        await _context.SaveChangesAsync();

        var result = await _repository.ObtenerTodosAdminAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task ObtenerPorTipoAsync_FiltraPorTipoProductoYActivo()
    {
        _context.Productos.Add(new Producto { Id = 1, Nombre = "P1", Precio = 100, Activo = true, TipoProductoId = 1 });
        _context.Productos.Add(new Producto { Id = 2, Nombre = "P2", Precio = 100, Activo = true, TipoProductoId = 2 });
        _context.Productos.Add(new Producto { Id = 3, Nombre = "P3", Precio = 100, Activo = false, TipoProductoId = 1 });
        await _context.SaveChangesAsync();

        var result = await _repository.ObtenerPorTipoAsync(1);

        Assert.Single(result);
        Assert.Equal("P1", result[0].Nombre);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_RetornaNuloSiNoExiste()
    {
        var result = await _repository.ObtenerPorIdAsync(999);
        Assert.Null(result);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_RetornaProductoConRelaciones()
    {
        var tipoProducto = new TipoProducto { Id = 1, Nombre = "Busito" };
        _context.TiposProducto.Add(tipoProducto);
        _context.Productos.Add(new Producto { Id = 1, Nombre = "Test", Precio = 100, TipoProductoId = 1 });
        await _context.SaveChangesAsync();

        var result = await _repository.ObtenerPorIdAsync(1);
        Assert.NotNull(result);
        Assert.Equal("Test", result!.Nombre);
        Assert.NotNull(result.TipoProducto);
    }

    [Fact]
    public async Task AgregarAsync_InsertaYRetornaConId()
    {
        var producto = new Producto { Nombre = "Nuevo", Precio = 200, Activo = true };

        var result = await _repository.AgregarAsync(producto);

        Assert.True(result.Id > 0);
        Assert.Equal(1, await _context.Productos.CountAsync());
    }

    [Fact]
    public async Task ActualizarAsync_ActualizaCampos()
    {
        var producto = new Producto { Nombre = "Original", Precio = 100, Activo = true };
        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        producto.Nombre = "Modificado";
        producto.Precio = 200;
        await _repository.ActualizarAsync(producto);

        var actualizado = await _context.Productos.FindAsync(producto.Id);
        Assert.Equal("Modificado", actualizado!.Nombre);
        Assert.Equal(200, actualizado.Precio);
    }

    [Fact]
    public async Task EliminarAsync_SoftDelete()
    {
        var producto = new Producto { Nombre = "AEliminar", Precio = 100, Activo = true };
        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        await _repository.EliminarAsync(producto.Id);

        var eliminado = await _context.Productos.FindAsync(producto.Id);
        Assert.NotNull(eliminado);
        Assert.False(eliminado!.Activo);
    }

#pragma warning disable xUnit1013
    public void Dispose() => _context.Dispose();
#pragma warning restore xUnit1013
}
