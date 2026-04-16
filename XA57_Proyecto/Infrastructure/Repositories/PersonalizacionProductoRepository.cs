using Microsoft.EntityFrameworkCore;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Domain.Interfaces;
using XA57_Proyecto.Infrastructure.Data;

namespace XA57_Proyecto.Infrastructure.Repositories
{
    public class PersonalizacionProductoRepository : IPersonalizacionProductoRepository
    {
        private readonly AppDbContext _context;

        public PersonalizacionProductoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PersonalizacionProducto?> GetByIdAsync(int id) =>
            await _context.PersonalizacionesProducto.FindAsync(id);

        public async Task<IReadOnlyList<PersonalizacionProducto>> ListAllAsync() =>
            await _context.PersonalizacionesProducto.ToListAsync();

        public async Task<PersonalizacionProducto> AddAsync(PersonalizacionProducto personalizacion)
        {
            _context.PersonalizacionesProducto.Add(personalizacion);
            await _context.SaveChangesAsync();
            return personalizacion;
        }

        public async Task UpdateAsync(PersonalizacionProducto personalizacion)
        {
            _context.Entry(personalizacion).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(PersonalizacionProducto personalizacion)
        {
            _context.PersonalizacionesProducto.Remove(personalizacion);
            await _context.SaveChangesAsync();
        }
    }
}