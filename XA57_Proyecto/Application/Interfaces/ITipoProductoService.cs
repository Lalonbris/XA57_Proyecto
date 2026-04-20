using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Application.Interfaces
{
    public interface ITipoProductoService
    {
        Task<List<TipoProducto>> ObtenerTodosAsync();
        Task<TipoProducto?> ObtenerPorIdAsync(int id);
        Task<TipoProducto> AgregarAsync(TipoProducto tipoProducto);
        Task ActualizarAsync(TipoProducto tipoProducto);
        Task EliminarAsync(int id);
    }
}
