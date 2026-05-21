using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Infrastructure.Repositories.Interfaces
{
    public interface IPedidoRepository
    {
        Task AgregarAsync(Pedido pedido);
        Task<List<Pedido>> ObtenerConProductosAsync();
        Task<List<Pedido>> ObtenerTodosAsync();
        Task<Pedido?> ObtenerPorIdAsync(int id);
        Task EliminarAsync(int id);
        Task ActualizarEstadoAsync(int id, string estado);
        Task ActualizarAsync(Pedido pedido);
        Task<int> ContarAsync(string? estado = null);
        Task<List<Pedido>> ObtenerPorUsuarioIdAsync(string userId, string estado);
        Task<int> ContarPorUsuarioIdAsync(string userId, string estado);
    }
}
