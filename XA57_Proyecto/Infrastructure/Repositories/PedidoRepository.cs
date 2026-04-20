using Microsoft.EntityFrameworkCore;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Infrastructure.Data;
using XA57_Proyecto.Infrastructure.Repositories.Interfaces;

namespace XA57_Proyecto.Infrastructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _context;

        public PedidoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AgregarAsync(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
        }

        public Task<List<Pedido>> ObtenerConProductosAsync() =>
            _context.Pedidos
                .Include(p => p.Producto)
                .Include(p => p.ModeloAutobus)
                .Include(p => p.Linea)
                .ToListAsync();

        public async Task EliminarAsync(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
            }
        }

        public Task<int> ContarAsync() => _context.Pedidos.CountAsync();

        public Task<List<Pedido>> ObtenerTodosAsync() =>
            _context.Pedidos
                .Include(p => p.Producto)
                .Include(p => p.ModeloAutobus)
                .Include(p => p.Linea)
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();

        public async Task<Pedido?> ObtenerPorIdAsync(int id) =>
            await _context.Pedidos
                .Include(p => p.Producto)
                .Include(p => p.ModeloAutobus)
                .Include(p => p.Linea)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task ActualizarEstadoAsync(int id, string estado)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido != null)
            {
                pedido.Estado = estado;
                await _context.SaveChangesAsync();
            }
        }
    }
}
