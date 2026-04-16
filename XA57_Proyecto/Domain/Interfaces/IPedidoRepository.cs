using System.Collections.Generic;
using System.Threading.Tasks;
using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Task<IReadOnlyList<Pedido>> GetAllWithProductsAsync();
        Task<Pedido?> GetByIdAsync(int id);
        Task<Pedido> AddAsync(Pedido pedido);
        Task UpdateAsync(Pedido pedido);
        Task DeleteAsync(int id);
        Task<int> CountAsync();
    }
}