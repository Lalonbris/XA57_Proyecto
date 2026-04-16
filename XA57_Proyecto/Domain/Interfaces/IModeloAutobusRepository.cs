using System.Collections.Generic;
using System.Threading.Tasks;
using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Domain.Interfaces
{
    public interface IModeloAutobusRepository
    {
        Task<IReadOnlyList<ModeloAutobus>> GetAllActiveAsync();
        Task<ModeloAutobus?> GetByIdAsync(int id);
        Task<ModeloAutobus> AddAsync(ModeloAutobus modelo);
        Task UpdateAsync(ModeloAutobus modelo);
        Task DeleteAsync(ModeloAutobus modelo);
    }
}