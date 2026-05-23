using Microsoft.EntityFrameworkCore;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Infrastructure.Data;
using XA57_Proyecto.Infrastructure.Repositories;

namespace XA57_Proyecto.Tests.Repositories;

public class PedidoRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly PedidoRepository _repository;

    public PedidoRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase($"TestDb_Pedidos_{Guid.NewGuid()}")
            .Options;

        _context = new AppDbContext(options);
        _repository = new PedidoRepository(_context);
    }

    [Fact]
    public async Task AgregarAsync_InsertaPedido()
    {
        var pedido = new Pedido
        {
            ProductoId = 1,
            ModeloAutobusId = 1,
            LineaId = 1,
            Cantidad = 2,
            Estado = "Recibido"
        };

        await _repository.AgregarAsync(pedido);

        Assert.True(pedido.Id > 0);
        Assert.Equal(1, await _context.Pedidos.CountAsync());
    }

    [Fact]
    public async Task ObtenerConProductosAsync_RetornaConRelaciones()
    {
        var producto = new Producto { Id = 1, Nombre = "Busito", Precio = 100, Activo = true };
        var modelo = new ModeloAutobus { Id = 1, Nombre = "Irizar i8", Fabricante = "Irizar", Activo = true };
        var linea = new Linea { Id = 1, Nombre = "ETN", Activa = true };
        _context.Productos.Add(producto);
        _context.ModelosAutobus.Add(modelo);
        _context.Lineas.Add(linea);

        var pedido = new Pedido
        {
            ProductoId = 1,
            ModeloAutobusId = 1,
            LineaId = 1,
            Cantidad = 1,
            Estado = "Recibido"
        };
        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        var result = await _repository.ObtenerConProductosAsync();

        Assert.Single(result);
        Assert.NotNull(result[0].Producto);
        Assert.Equal("Busito", result[0].Producto!.Nombre);
        Assert.NotNull(result[0].ModeloAutobus);
        Assert.NotNull(result[0].Linea);
    }

    [Fact]
    public async Task ObtenerTodosAsync_RetornaOrdenadoPorFechaDesc()
    {
        var producto = new Producto { Id = 1, Nombre = "Test", Precio = 100, Activo = true };
        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        _context.Pedidos.Add(new Pedido { ProductoId = 1, Cantidad = 1, Estado = "Recibido", FechaCreacion = DateTime.UtcNow.AddDays(-2) });
        _context.Pedidos.Add(new Pedido { ProductoId = 1, Cantidad = 1, Estado = "Recibido", FechaCreacion = DateTime.UtcNow });
        await _context.SaveChangesAsync();

        var result = await _repository.ObtenerTodosAsync();

        Assert.Equal(2, result.Count);
        Assert.True(result[0].FechaCreacion >= result[1].FechaCreacion);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_RetornaConRelaciones()
    {
        var producto = new Producto { Id = 1, Nombre = "Test", Precio = 100, Activo = true };
        _context.Productos.Add(producto);
        var pedido = new Pedido { Id = 1, ProductoId = 1, Cantidad = 1, Estado = "Recibido" };
        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        var result = await _repository.ObtenerPorIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Test", result!.Producto!.Nombre);
    }

    [Fact]
    public async Task EliminarAsync_HardDelete()
    {
        var pedido = new Pedido { ProductoId = 1, Cantidad = 1, Estado = "Recibido" };
        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        await _repository.EliminarAsync(pedido.Id);

        var eliminado = await _context.Pedidos.FindAsync(pedido.Id);
        Assert.Null(eliminado);
    }

    [Fact]
    public async Task ContarAsync_RetornaConteoCorrecto()
    {
        _context.Pedidos.Add(new Pedido { ProductoId = 1, Cantidad = 1, Estado = "Recibido" });
        _context.Pedidos.Add(new Pedido { ProductoId = 2, Cantidad = 1, Estado = "Recibido" });
        await _context.SaveChangesAsync();

        var count = await _repository.ContarAsync();
        Assert.Equal(2, count);
    }

    [Fact]
    public async Task ActualizarEstadoAsync_CambiaEstado()
    {
        var pedido = new Pedido { ProductoId = 1, Cantidad = 1, Estado = "Recibido" };
        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        await _repository.ActualizarEstadoAsync(pedido.Id, "En producción");

        var actualizado = await _context.Pedidos.FindAsync(pedido.Id);
        Assert.Equal("En producción", actualizado!.Estado);
    }

    // ============ RN-05: Integridad del Texto ============

    [Fact]
    public async Task AgregarAsync_GuardaTextoCompletoSinTruncamiento()
    {
        var pedido = new Pedido
        {
            ProductoId = 1,
            NombreOperador = "Expreso Futura",
            NumeroEconomico = "105-A",
            Ruta = "México - Guadalajara",
            NotasEspeciales = "Color especial de llanta, sublimar logo en el frente",
            Cantidad = 1,
            Estado = "Recibido"
        };

        await _repository.AgregarAsync(pedido);

        var guardado = await _context.Pedidos.FindAsync(pedido.Id);
        Assert.Equal("Expreso Futura", guardado!.NombreOperador);
        Assert.Equal("105-A", guardado.NumeroEconomico);
        Assert.Equal("México - Guadalajara", guardado.Ruta);
        Assert.Equal("Color especial de llanta, sublimar logo en el frente", guardado.NotasEspeciales);
    }

    public void Dispose() => _context.Dispose();
}
