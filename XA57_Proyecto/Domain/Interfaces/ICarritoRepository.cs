using System.Collections.Generic;
using System.Threading.Tasks;
using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Domain.Interfaces
{
    public interface ICarritoRepository
    {
        Task<Carrito?> GetByIdAsync(int id);
        Task<Carrito?> GetByUsuarioIdAsync(int usuarioId);
        Task<Carrito> AddAsync(Carrito carrito);
        Task UpdateAsync(Carrito carrito);
        Task DeleteAsync(Carrito carrito);
    }
}