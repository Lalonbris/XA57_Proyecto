using Microsoft.EntityFrameworkCore;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Infrastructure.Data;
using XA57_Proyecto.Infrastructure.Repositories.Interfaces;

namespace XA57_Proyecto.Infrastructure.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly AppDbContext _context;

        public ProductoRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<Producto>> ObtenerTodosAsync() =>
            _context.Productos
                .Where(p => p.Activo)
                .Include(p => p.TipoProducto)
                .Include(p => p.Linea)
                .ToListAsync();

        public Task<List<Producto>> ObtenerTodosAdminAsync() =>
            _context.Productos
                .Include(p => p.TipoProducto)
                .Include(p => p.Linea)
                .ToListAsync();

        public Task<List<Producto>> ObtenerPorTipoAsync(int tipoProductoId) =>
            _context.Productos
                .Where(p => p.Activo && p.TipoProductoId == tipoProductoId)
                .Include(p => p.TipoProducto)
                .Include(p => p.Linea)
                .ToListAsync();

        public async Task<Producto?> ObtenerPorIdAsync(int id) =>
            await _context.Productos
                .Include(p => p.TipoProducto)
                .Include(p => p.Linea)
                .FirstOrDefaultAsync(p => p.Id == id);
        
        public async Task<Producto> AgregarAsync(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        public async Task ActualizarAsync(Producto producto)
        {
            var productoExistente = await _context.Productos.FindAsync(producto.Id);
            if (productoExistente == null)
            {
                throw new InvalidOperationException("Producto no encontrado");
            }

            productoExistente.Nombre = producto.Nombre;
            productoExistente.Descripcion = producto.Descripcion;
            productoExistente.Precio = producto.Precio;
            productoExistente.ImagenUrl = producto.ImagenUrl;
            productoExistente.TipoProductoId = producto.TipoProductoId;
            productoExistente.LineaId = producto.LineaId;
            productoExistente.Tamano = producto.Tamano;
            productoExistente.Activo = producto.Activo;

            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                producto.Activo = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}
