using System.Collections.Generic;
using System.Threading.Tasks;
using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Domain.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<Categoria?> GetByIdAsync(int id);
        Task<IReadOnlyList<Categoria>> ListAllAsync();
        Task<Categoria> AddAsync(Categoria categoria);
        Task UpdateAsync(Categoria categoria);
        Task DeleteAsync(Categoria categoria);
    }
}