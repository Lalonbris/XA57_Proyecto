using Microsoft.EntityFrameworkCore;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Domain.Interfaces;
using XA57_Proyecto.Infrastructure.Data;

namespace XA57_Proyecto.Infrastructure.Repositories
{
    public class ModeloAutobusRepository : IModeloAutobusRepository
    {
        private readonly AppDbContext _context;

        public ModeloAutobusRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ModeloAutobus>> GetAllActiveAsync() =>
            await _context.ModelosAutobus
                .Where(m => m.Activo)
                .OrderBy(m => m.Nombre)
                .ToListAsync();

        public async Task<ModeloAutobus?> GetByIdAsync(int id) =>
            await _context.ModelosAutobus.FindAsync(id);

        public async Task<ModeloAutobus> AddAsync(ModeloAutobus modelo)
        {
            _context.ModelosAutobus.Add(modelo);
            await _context.SaveChangesAsync();
            return modelo;
        }

        public async Task UpdateAsync(ModeloAutobus modelo)
        {
            _context.Entry(modelo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ModeloAutobus modelo)
        {
            _context.ModelosAutobus.Remove(modelo);
            await _context.SaveChangesAsync();
        }
    }
}