using System.Collections.Generic;
using System.Threading.Tasks;
using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Domain.Interfaces
{
    public interface IProductoRepository
    {
        Task<IReadOnlyList<Producto>> GetAllAsync();
        Task<Producto?> GetByIdAsync(int id);
        Task<Producto> AddAsync(Producto producto);
        Task UpdateAsync(Producto producto);
        Task DeleteAsync(Producto producto);
    }
}