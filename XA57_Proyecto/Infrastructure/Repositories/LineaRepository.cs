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

        public Task<List<Linea>> ObtenerTodosAsync() =>
            _context.Lineas.OrderBy(l => l.Nombre).ToListAsync();

        public Task<List<Linea>> ObtenerActivasAsync() =>
            _context.Lineas.Where(l => l.Activa).OrderBy(l => l.Nombre).ToListAsync();

        public async Task<Linea?> ObtenerPorIdAsync(int id) =>
            await _context.Lineas.FindAsync(id);

        public async Task<Linea> AgregarAsync(Linea linea)
        {
            _context.Lineas.Add(linea);
            await _context.SaveChangesAsync();
            return linea;
        }

        public async Task ActualizarAsync(Linea linea)
        {
            _context.Entry(linea).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var linea = await _context.Lineas.FindAsync(id);
            if (linea != null)
            {
                linea.Activa = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}
