using XA57_Proyecto.Application.DTOs;
using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Application.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoResultDto> AgregarAsync(CarritoItemDto item);
        Task<List<Pedido>> ObtenerCarritoAsync();
        Task EliminarItemAsync(int id);
        Task<int> ContarItemsAsync();
        Task<List<Pedido>> ObtenerTodosAsync();
        Task<Pedido?> ObtenerPorIdAsync(int id);
        Task ActualizarEstadoAsync(int id, string estado);
    }
}
