using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Application.Interfaces
{
    public interface ILineaService
    {
        Task<List<Linea>> ObtenerActivasAsync();
        Task<Linea?> ObtenerPorIdAsync(int id);
    }
}
