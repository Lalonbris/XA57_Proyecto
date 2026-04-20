using Microsoft.EntityFrameworkCore;
using XA57_Proyecto.Application.Interfaces;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Infrastructure.Repositories.Interfaces;

namespace XA57_Proyecto.Application.Services
{
    public class TipoProductoService : ITipoProductoService
    {
        private readonly ITipoProductoRepository _repo;

        public TipoProductoService(ITipoProductoRepository repo)
        {
            _repo = repo;
        }

        public Task<List<TipoProducto>> ObtenerTodosAsync() => _repo.ObtenerTodosAsync();

        public Task<TipoProducto?> ObtenerPorIdAsync(int id) => _repo.ObtenerPorIdAsync(id);

        public async Task<TipoProducto> AgregarAsync(TipoProducto tipoProducto)
        {
            return await _repo.AgregarAsync(tipoProducto);
        }

        public async Task ActualizarAsync(TipoProducto tipoProducto)
        {
            await _repo.ActualizarAsync(tipoProducto);
        }

        public async Task EliminarAsync(int id)
        {
            await _repo.EliminarAsync(id);
        }
    }
}