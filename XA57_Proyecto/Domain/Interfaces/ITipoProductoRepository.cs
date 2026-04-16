using System.Collections.Generic;
using System.Threading.Tasks;
using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Domain.Interfaces
{
    public interface ITipoProductoRepository
    {
        Task<IReadOnlyList<TipoProducto>> GetAllAsync();
        Task<TipoProducto?> GetByIdAsync(int id);
        Task<TipoProducto> AddAsync(TipoProducto tipoProducto);
        Task UpdateAsync(TipoProducto tipoProducto);
        Task DeleteAsync(TipoProducto tipoProducto);
    }
}