using System.Collections.Generic;
using System.Threading.Tasks;
using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Domain.Interfaces
{
    public interface ILineaRepository
    {
        Task<IReadOnlyList<Linea>> GetAllActiveAsync();
        Task<Linea?> GetByIdAsync(int id);
        Task<Linea> AddAsync(Linea linea);
        Task UpdateAsync(Linea linea);
        Task DeleteAsync(Linea linea);
    }
}