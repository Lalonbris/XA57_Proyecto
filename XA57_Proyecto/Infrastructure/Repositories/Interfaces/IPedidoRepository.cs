using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Infrastructure.Repositories.Interfaces
{
    public interface IPedidoRepository
    {
        Task AgregarAsync(Pedido pedido);
        Task<List<Pedido>> ObtenerConProductosAsync();
        Task<Pedido?> ObtenerPorIdAsync(int id);
        Task ActualizarAsync(Pedido pedido);
        Task EliminarAsync(int id);
        Task<int> ContarAsync();
    }
}
