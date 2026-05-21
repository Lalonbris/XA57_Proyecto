using XA57_Proyecto.Application.DTOs;
using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Application.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoResultDto> AgregarAsync(CarritoItemDto item, string userId);
        Task<List<Pedido>> ObtenerCarritoAsync(string userId);
        Task EliminarItemAsync(int id);
        Task<int> ContarItemsAsync(string userId);
        Task<List<Pedido>> ObtenerTodosAsync();
        Task<Pedido?> ObtenerPorIdAsync(int id);
        Task ActualizarEstadoAsync(int id, string estado);
    }
}
