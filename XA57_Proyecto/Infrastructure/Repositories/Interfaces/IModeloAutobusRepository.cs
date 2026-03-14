using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Infrastructure.Repositories.Interfaces
{
    public interface IModeloAutobusRepository
    {
        Task<List<ModeloAutobus>> ObtenerActivosAsync();
        Task<ModeloAutobus?> ObtenerPorIdAsync(int id);
    }
}
