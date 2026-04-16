using Microsoft.EntityFrameworkCore;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Domain.Interfaces;
using XA57_Proyecto.Infrastructure.Data;

namespace XA57_Proyecto.Infrastructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _context;

        public PedidoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Pedido>> GetAllWithProductsAsync() =>
            await _context.Pedidos
                .Include(p => p.Producto)
                .Include(p => p.ModeloAutobus)
                .Include(p => p.Linea)
                .ToListAsync();

        public async Task<Pedido?> GetByIdAsync(int id) =>
            await _context.Pedidos
                .Include(p => p.Producto)
                .Include(p => p.ModeloAutobus)
                .Include(p => p.Linea)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Pedido> AddAsync(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
            return pedido;
        }

        public async Task UpdateAsync(Pedido pedido)
        {
            _context.Entry(pedido).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> CountAsync() => await _context.Pedidos.CountAsync();
    }
}