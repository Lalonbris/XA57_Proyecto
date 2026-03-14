using XA57_Proyecto.Application.Interfaces;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Infrastructure.Repositories.Interfaces;

namespace XA57_Proyecto.Application.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repo;

        public ProductoService(IProductoRepository repo)
        {
            _repo = repo;
        }

        public Task<List<Producto>> ObtenerTodosAsync() => _repo.ObtenerTodosAsync();

        public Task<Producto?> ObtenerPorIdAsync(int id) => _repo.ObtenerPorIdAsync(id);
    }
}
