using Microsoft.EntityFrameworkCore;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Domain.Interfaces;
using XA57_Proyecto.Infrastructure.Data;

namespace XA57_Proyecto.Infrastructure.Repositories
{
    public class ItemOrdenRepository : IItemOrdenRepository
    {
        private readonly AppDbContext _context;

        public ItemOrdenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ItemOrden?> GetByIdAsync(int id) =>
            await _context.ItemsOrden
                .Include(i => i.Producto)
                .Include(i => i.Personalizacion)
                .FirstOrDefaultAsync(i => i.Id == id);

        public async Task<IReadOnlyList<ItemOrden>> GetByOrdenIdAsync(int ordenId) =>
            await _context.ItemsOrden
                .Include(i => i.Producto)
                .Include(i => i.Personalizacion)
                .Where(i => i.OrdenId == ordenId)
                .ToListAsync();

        public async Task<ItemOrden> AddAsync(ItemOrden itemOrden)
        {
            _context.ItemsOrden.Add(itemOrden);
            await _context.SaveChangesAsync();
            return itemOrden;
        }

        public async Task UpdateAsync(ItemOrden itemOrden)
        {
            _context.Entry(itemOrden).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ItemOrden itemOrden)
        {
            _context.ItemsOrden.Remove(itemOrden);
            await _context.SaveChangesAsync();
        }
    }
}