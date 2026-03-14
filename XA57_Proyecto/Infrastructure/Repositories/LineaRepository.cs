using Microsoft.EntityFrameworkCore;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Infrastructure.Data;
using XA57_Proyecto.Infrastructure.Repositories.Interfaces;

namespace XA57_Proyecto.Infrastructure.Repositories
{
    public class LineaRepository : ILineaRepository
    {
        private readonly AppDbContext _context;

        public LineaRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<Linea>> ObtenerActivasAsync() =>
            _context.Lineas.Where(l => l.Activa).OrderBy(l => l.Nombre).ToListAsync();

        public async Task<Linea?> ObtenerPorIdAsync(int id) =>
            await _context.Lineas.FindAsync(id);
    }
}
