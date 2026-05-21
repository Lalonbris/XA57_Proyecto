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
        Task<int> ContarAsync();
        Task ActualizarEstadoAsync(int id, string estado);
    }
}
