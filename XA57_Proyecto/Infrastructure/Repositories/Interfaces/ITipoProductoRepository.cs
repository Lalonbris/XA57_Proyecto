using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Infrastructure.Repositories.Interfaces
{
    public interface ITipoProductoRepository
    {
        Task<TipoProducto?> ObtenerPorIdAsync(int id);
    }
}
