using System.Collections.Generic;
using System.Threading.Tasks;
using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Domain.Interfaces
{
    public interface IPersonalizacionProductoRepository
    {
        Task<PersonalizacionProducto?> GetByIdAsync(int id);
        Task<IReadOnlyList<PersonalizacionProducto>> ListAllAsync();
        Task<PersonalizacionProducto> AddAsync(PersonalizacionProducto personalizacion);
        Task UpdateAsync(PersonalizacionProducto personalizacion);
        Task DeleteAsync(PersonalizacionProducto personalizacion);
    }
}