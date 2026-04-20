using Microsoft.EntityFrameworkCore;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Infrastructure.Data;
using XA57_Proyecto.Infrastructure.Repositories.Interfaces;

namespace XA57_Proyecto.Infrastructure.Repositories
{
    public class TipoProductoRepository : ITipoProductoRepository
    {
        private readonly AppDbContext _context;

        public TipoProductoRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<TipoProducto>> ObtenerTodosAsync()
        {
            return _context.TiposProducto.ToListAsync();
        }

        public Task<TipoProducto?> ObtenerPorIdAsync(int id)
        {
            return _context.TiposProducto.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TipoProducto> AgregarAsync(TipoProducto tipoProducto)
        {
            _context.TiposProducto.Add(tipoProducto);
            await _context.SaveChangesAsync();
            return tipoProducto;
        }

        public async Task ActualizarAsync(TipoProducto tipoProducto)
        {
            _context.Entry(tipoProducto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var tipoProducto = await _context.TiposProducto.FindAsync(id);
            if (tipoProducto != null)
            {
                _context.TiposProducto.Remove(tipoProducto);
                await _context.SaveChangesAsync();
            }
        }
    }
}