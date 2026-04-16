using XA57_Proyecto.Application.DTOs;
using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Application.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoResultDto> AddAsync(CarritoItemDto item);
        Task<IReadOnlyList<Pedido>> GetAllAsync();
        Task DeleteAsync(int id);
        Task<int> CountAsync();
    }
}