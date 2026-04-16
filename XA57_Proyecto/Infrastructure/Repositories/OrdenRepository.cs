using Microsoft.EntityFrameworkCore;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Domain.Interfaces;
using XA57_Proyecto.Infrastructure.Data;

namespace XA57_Proyecto.Infrastructure.Repositories
{
    public class OrdenRepository : IOrdenRepository
    {
        private readonly AppDbContext _context;

        public OrdenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Orden?> GetByIdAsync(int id) =>
            await _context.Ordenes
                .Include(o => o.Items)
                .ThenInclude(i => i.Producto)
                .FirstOrDefaultAsync(o => o.Id == id);

        public async Task<IReadOnlyList<Orden>> ListAllAsync() =>
            await _context.Ordenes
                .Include(o => o.Items)
                .ToListAsync();

        public async Task<IReadOnlyList<Orden>> GetByUsuarioIdAsync(int usuarioId) =>
            await _context.Ordenes
                .Include(o => o.Items)
                .ThenInclude(i => i.Producto)
                .Where(o => o.UsuarioId == usuarioId)
                .ToListAsync();

        public async Task<Orden> AddAsync(Orden orden)
        {
            _context.Ordenes.Add(orden);
            await _context.SaveChangesAsync();
            return orden;
        }

        public async Task UpdateAsync(Orden orden)
        {
            _context.Entry(orden).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Orden orden)
        {
            _context.Ordenes.Remove(orden);
            await _context.SaveChangesAsync();
        }
    }
}