using System.Collections.Generic;
using System.Threading.Tasks;
using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Domain.Interfaces
{
    public interface IOrdenRepository
    {
        Task<Orden?> GetByIdAsync(int id);
        Task<IReadOnlyList<Orden>> ListAllAsync();
        Task<IReadOnlyList<Orden>> GetByUsuarioIdAsync(int usuarioId);
        Task<Orden> AddAsync(Orden orden);
        Task UpdateAsync(Orden orden);
        Task DeleteAsync(Orden orden);
    }
}