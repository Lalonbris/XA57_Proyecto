using System.Collections.Generic;
using System.Threading.Tasks;
using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Domain.Interfaces
{
    public interface IItemOrdenRepository
    {
        Task<ItemOrden?> GetByIdAsync(int id);
        Task<IReadOnlyList<ItemOrden>> GetByOrdenIdAsync(int ordenId);
        Task<ItemOrden> AddAsync(ItemOrden itemOrden);
        Task UpdateAsync(ItemOrden itemOrden);
        Task DeleteAsync(ItemOrden itemOrden);
    }
}