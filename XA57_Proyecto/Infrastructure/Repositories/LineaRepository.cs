using Microsoft.EntityFrameworkCore;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Domain.Interfaces;
using XA57_Proyecto.Infrastructure.Data;

namespace XA57_Proyecto.Infrastructure.Repositories
{
    public class LineaRepository : ILineaRepository
    {
        private readonly AppDbContext _context;

        public LineaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Linea>> GetAllActiveAsync() =>
            await _context.Lineas
                .Where(l => l.Activa)
                .OrderBy(l => l.Nombre)
                .ToListAsync();

        public async Task<Linea?> GetByIdAsync(int id) =>
            await _context.Lineas.FindAsync(id);

        public async Task<Linea> AddAsync(Linea linea)
        {
            _context.Lineas.Add(linea);
            await _context.SaveChangesAsync();
            return linea;
        }

        public async Task UpdateAsync(Linea linea)
        {
            _context.Entry(linea).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Linea linea)
        {
            _context.Lineas.Remove(linea);
            await _context.SaveChangesAsync();
        }
    }
}