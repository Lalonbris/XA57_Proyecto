using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Application.Interfaces
{
    public interface ILineaService
    {
        Task<IReadOnlyList<Linea>> GetAllActiveAsync();
        Task<Linea?> GetByIdAsync(int id);
    }
}