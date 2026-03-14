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
                .ToListAsync();

        public async Task<Producto?> ObtenerPorIdAsync(int id) =>
            await _context.Productos
                .Include(p => p.TipoProducto)
                .FirstOrDefaultAsync(p => p.Id == id);
        
        public async Task<Producto> AgregarAsync(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        public async Task ActualizarAsync(Producto producto)
        {
            _context.Entry(producto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
