using XA57_Proyecto.Application.Exceptions;
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
            ValidarTipoProducto(tipoProducto);
            return await _repo.AgregarAsync(tipoProducto);
        }

        public async Task ActualizarAsync(TipoProducto tipoProducto)
        {
            var existente = await _repo.ObtenerPorIdAsync(tipoProducto.Id);
            if (existente == null)
            {
                throw new ValidationException("El tipo de producto que intenta actualizar no existe.");
            }
            ValidarTipoProducto(tipoProducto);
            await _repo.ActualizarAsync(tipoProducto);
        }

        public async Task EliminarAsync(int id)
        {
            var tipo = await _repo.ObtenerPorIdAsync(id);
            if (tipo == null)
            {
                throw new ValidationException("El tipo de producto que intenta eliminar no existe.");
            }
            await _repo.EliminarAsync(id);
        }

        private static void ValidarTipoProducto(TipoProducto tipoProducto)
        {
            if (string.IsNullOrWhiteSpace(tipoProducto.Nombre))
            {
                throw new ValidationException("El nombre del tipo de producto no puede estar vacío.");
            }

            if (tipoProducto.Nombre.Length > 100)
            {
                throw new ValidationException("El nombre del tipo de producto no puede exceder los 100 caracteres.");
            }

            if (tipoProducto.MaxCaracteres < 1 || tipoProducto.MaxCaracteres > 100)
            {
                throw new ValidationException("El máximo de caracteres debe estar entre 1 y 100.");
            }
        }
    }
}