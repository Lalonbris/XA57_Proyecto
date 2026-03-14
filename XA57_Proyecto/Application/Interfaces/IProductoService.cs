using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Application.Interfaces
{
    public interface IProductoService
    {
        Task<List<Producto>> ObtenerTodosAsync();
        Task<Producto?> ObtenerPorIdAsync(int id);
    }
}
