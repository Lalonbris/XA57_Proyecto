using Microsoft.EntityFrameworkCore;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Infrastructure.Data;
using XA57_Proyecto.Infrastructure.Repositories.Interfaces;

namespace XA57_Proyecto.Infrastructure.Repositories
{
    public class ModeloAutobusRepository : IModeloAutobusRepository
    {
        private readonly AppDbContext _context;

        public ModeloAutobusRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<ModeloAutobus>> ObtenerActivosAsync() =>
            _context.ModelosAutobus.Where(m => m.Activo).OrderBy(m => m.Nombre).ToListAsync();

        public async Task<ModeloAutobus?> ObtenerPorIdAsync(int id) =>
            await _context.ModelosAutobus.FindAsync(id);
    }
}
