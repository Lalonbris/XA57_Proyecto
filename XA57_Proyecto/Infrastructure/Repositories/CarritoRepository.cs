using Microsoft.EntityFrameworkCore;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Domain.Interfaces;
using XA57_Proyecto.Infrastructure.Data;

namespace XA57_Proyecto.Infrastructure.Repositories
{
    public class CarritoRepository : ICarritoRepository
    {
        private readonly AppDbContext _context;

        public CarritoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Carrito?> GetByIdAsync(int id) =>
            await _context.Carritos
                .Include(c => c.Items)
                .ThenInclude(i => i.Producto)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Carrito?> GetByUsuarioIdAsync(int usuarioId) =>
            await _context.Carritos
                .Include(c => c.Items)
                .ThenInclude(i => i.Producto)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

        public async Task<Carrito> AddAsync(Carrito carrito)
        {
            _context.Carritos.Add(carrito);
            await _context.SaveChangesAsync();
            return carrito;
        }

        public async Task UpdateAsync(Carrito carrito)
        {
            _context.Entry(carrito).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Carrito carrito)
        {
            _context.Carritos.Remove(carrito);
            await _context.SaveChangesAsync();
        }
    }
}