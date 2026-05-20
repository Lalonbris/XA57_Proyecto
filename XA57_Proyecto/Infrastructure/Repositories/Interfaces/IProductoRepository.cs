using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Infrastructure.Repositories.Interfaces
{
    public interface IProductoRepository
    {
        Task<List<Producto>> ObtenerTodosAsync();
        Task<List<Producto>> ObtenerTodosAdminAsync();
        Task<List<Producto>> ObtenerPorTipoAsync(int tipoProductoId);
        Task<Producto?> ObtenerPorIdAsync(int id);
        Task<Producto> AgregarAsync(Producto producto);
        Task ActualizarAsync(Producto producto);
        Task EliminarAsync(int id);
    }
}
