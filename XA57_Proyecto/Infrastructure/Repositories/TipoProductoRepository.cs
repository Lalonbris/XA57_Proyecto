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

        public Task<TipoProducto?> ObtenerPorIdAsync(int id)
        {
            return _context.TiposProducto.FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
