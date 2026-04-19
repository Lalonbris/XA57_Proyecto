using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Application.Interfaces
{
    public interface IProductoService
    {
        Task<List<Producto>> ObtenerTodosAsync();
        Task<List<Producto>> ObtenerPorTipoAsync(int tipoProductoId);
        Task<Producto?> ObtenerPorIdAsync(int id);
        Task<Producto> AgregarAsync(Producto producto);
        Task ActualizarAsync(Producto producto);
    }
}
