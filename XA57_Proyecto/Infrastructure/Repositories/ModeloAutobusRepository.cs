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

        public Task<List<ModeloAutobus>> ObtenerTodosAsync() =>
            _context.ModelosAutobus.OrderBy(m => m.Nombre).ToListAsync();

        public Task<List<ModeloAutobus>> ObtenerActivosAsync() =>
            _context.ModelosAutobus.Where(m => m.Activo).OrderBy(m => m.Nombre).ToListAsync();

        public async Task<ModeloAutobus?> ObtenerPorIdAsync(int id) =>
            await _context.ModelosAutobus.FindAsync(id);

        public async Task<ModeloAutobus> AgregarAsync(ModeloAutobus modelo)
        {
            _context.ModelosAutobus.Add(modelo);
            await _context.SaveChangesAsync();
            return modelo;
        }

        public async Task ActualizarAsync(ModeloAutobus modelo)
        {
            var existente = await _context.ModelosAutobus.FindAsync(modelo.Id);
            if (existente == null) throw new InvalidOperationException("Modelo no encontrado");

            existente.Nombre = modelo.Nombre;
            existente.Fabricante = modelo.Fabricante;
            existente.Activo = modelo.Activo;

            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var modelo = await _context.ModelosAutobus.FindAsync(id);
            if (modelo != null)
            {
                modelo.Activo = false; // Soft delete por defecto
                await _context.SaveChangesAsync();
            }
        }
    }
}
