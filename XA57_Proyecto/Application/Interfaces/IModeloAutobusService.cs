using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Application.Interfaces
{
    public interface IModeloAutobusService
    {
        Task<IReadOnlyList<ModeloAutobus>> GetAllActiveAsync();
        Task<ModeloAutobus?> GetByIdAsync(int id);
    }
}