using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Application.Interfaces
{
    public interface IModeloAutobusService
    {
        Task<List<ModeloAutobus>> ObtenerActivosAsync();
        Task<ModeloAutobus?> ObtenerPorIdAsync(int id);
    }
}
