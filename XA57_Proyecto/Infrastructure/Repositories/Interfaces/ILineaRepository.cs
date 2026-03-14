using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Infrastructure.Repositories.Interfaces
{
    public interface ILineaRepository
    {
        Task<List<Linea>> ObtenerActivasAsync();
        Task<Linea?> ObtenerPorIdAsync(int id);
    }
}
