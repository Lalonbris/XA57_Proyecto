using Microsoft.EntityFrameworkCore;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Domain.Interfaces;
using XA57_Proyecto.Infrastructure.Data;

namespace XA57_Proyecto.Infrastructure.Repositories
{
    public class TipoProductoRepository : ITipoProductoRepository
    {
        private readonly AppDbContext _context;

        public TipoProductoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<TipoProducto>> GetAllAsync() =>
            await _context.TiposProducto.ToListAsync();

        public async Task<TipoProducto?> GetByIdAsync(int id) =>
            await _context.TiposProducto.FirstOrDefaultAsync(t => t.Id == id);

        public async Task<TipoProducto> AddAsync(TipoProducto tipoProducto)
        {
            _context.TiposProducto.Add(tipoProducto);
            await _context.SaveChangesAsync();
            return tipoProducto;
        }

        public async Task UpdateAsync(TipoProducto tipoProducto)
        {
            _context.Entry(tipoProducto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TipoProducto tipoProducto)
        {
            _context.TiposProducto.Remove(tipoProducto);
            await _context.SaveChangesAsync();
        }
    }
}