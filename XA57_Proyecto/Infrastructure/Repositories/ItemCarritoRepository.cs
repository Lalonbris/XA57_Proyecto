using Microsoft.EntityFrameworkCore;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Domain.Interfaces;
using XA57_Proyecto.Infrastructure.Data;

namespace XA57_Proyecto.Infrastructure.Repositories
{
    public class ItemCarritoRepository : IItemCarritoRepository
    {
        private readonly AppDbContext _context;

        public ItemCarritoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ItemCarrito?> GetByIdAsync(int id) =>
            await _context.ItemsCarrito
                .Include(i => i.Producto)
                .FirstOrDefaultAsync(i => i.Id == id);

        public async Task<IReadOnlyList<ItemCarrito>> GetByCarritoIdAsync(int carritoId) =>
            await _context.ItemsCarrito
                .Include(i => i.Producto)
                .Where(i => i.CarritoId == carritoId)
                .ToListAsync();

        public async Task<ItemCarrito> AddAsync(ItemCarrito itemCarrito)
        {
            _context.ItemsCarrito.Add(itemCarrito);
            await _context.SaveChangesAsync();
            return itemCarrito;
        }

        public async Task UpdateAsync(ItemCarrito itemCarrito)
        {
            _context.Entry(itemCarrito).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ItemCarrito itemCarrito)
        {
            _context.ItemsCarrito.Remove(itemCarrito);
            await _context.SaveChangesAsync();
        }
    }
}