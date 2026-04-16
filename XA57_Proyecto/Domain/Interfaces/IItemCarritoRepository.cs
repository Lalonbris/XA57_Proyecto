using System.Collections.Generic;
using System.Threading.Tasks;
using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Domain.Interfaces
{
    public interface IItemCarritoRepository
    {
        Task<ItemCarrito?> GetByIdAsync(int id);
        Task<IReadOnlyList<ItemCarrito>> GetByCarritoIdAsync(int carritoId);
        Task<ItemCarrito> AddAsync(ItemCarrito itemCarrito);
        Task UpdateAsync(ItemCarrito itemCarrito);
        Task DeleteAsync(ItemCarrito itemCarrito);
    }
}