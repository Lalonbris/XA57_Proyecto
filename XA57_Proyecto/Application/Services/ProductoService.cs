using XA57_Proyecto.Application.Exceptions;
using XA57_Proyecto.Application.Interfaces;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Infrastructure.Repositories.Interfaces;

namespace XA57_Proyecto.Application.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repo;
        private readonly ITipoProductoRepository _tipoProductoRepo;
        private readonly ILineaRepository _lineaRepo;

        public ProductoService(
            IProductoRepository repo, 
            ITipoProductoRepository tipoProductoRepo,
            ILineaRepository lineaRepo)
        {
            _repo = repo;
            _tipoProductoRepo = tipoProductoRepo;
            _lineaRepo = lineaRepo;
        }

        public Task<List<Producto>> ObtenerTodosAsync() => _repo.ObtenerTodosAsync();

        public Task<List<Producto>> ObtenerTodosAdminAsync() => _repo.ObtenerTodosAdminAsync();

        public Task<List<Producto>> ObtenerPorTipoAsync(int tipoProductoId) => _repo.ObtenerPorTipoAsync(tipoProductoId);

        public Task<Producto?> ObtenerPorIdAsync(int id) => _repo.ObtenerPorIdAsync(id);

        public async Task<Producto> AgregarAsync(Producto producto)
        {
            if (string.IsNullOrWhiteSpace(producto.Nombre))
            {
                throw new ValidationException("El nombre del producto no puede estar vacío.");
            }

            if (producto.Nombre.Length > 100)
            {
                throw new ValidationException("El nombre del producto no puede exceder los 100 caracteres.");
            }

            if (producto.Precio <= 0)
            {
                throw new ValidationException("El precio del producto debe ser mayor a cero.");
            }

            if (producto.TipoProductoId.HasValue)
            {
                var tipoProducto = await _tipoProductoRepo.ObtenerPorIdAsync(producto.TipoProductoId.Value);
                if (tipoProducto == null)
                {
                    throw new ValidationException("El tipo de producto especificado no es válido.");
                }
            }

            if (producto.LineaId.HasValue)
            {
                var linea = await _lineaRepo.ObtenerPorIdAsync(producto.LineaId.Value);
                if (linea == null)
                {
                    throw new ValidationException("La línea especificada no es válida.");
                }
            }

            return await _repo.AgregarAsync(producto);
        }

        public async Task ActualizarAsync(Producto producto)
        {
            var productoExistente = await _repo.ObtenerPorIdAsync(producto.Id);
            if (productoExistente == null)
            {
                throw new ValidationException("El producto que intenta actualizar no existe.");
            }

            if (string.IsNullOrWhiteSpace(producto.Nombre))
            {
                throw new ValidationException("El nombre del producto no puede estar vacío.");
            }

            if (producto.Nombre.Length > 100)
            {
                throw new ValidationException("El nombre del producto no puede exceder los 100 caracteres.");
            }

            if (producto.Precio <= 0)
            {
                throw new ValidationException("El precio del producto debe ser mayor a cero.");
            }

            if (producto.TipoProductoId.HasValue)
            {
                var tipoProducto = await _tipoProductoRepo.ObtenerPorIdAsync(producto.TipoProductoId.Value);
                if (tipoProducto == null)
                {
                    throw new ValidationException("El tipo de producto especificado no es válido.");
                }
            }

            if (producto.LineaId.HasValue)
            {
                var linea = await _lineaRepo.ObtenerPorIdAsync(producto.LineaId.Value);
                if (linea == null)
                {
                    throw new ValidationException("La línea especificada no es válida.");
                }
            }

            await _repo.ActualizarAsync(producto);
        }

        public async Task EliminarAsync(int id)
        {
            var producto = await _repo.ObtenerPorIdAsync(id);
            if (producto == null)
            {
                throw new ValidationException("El producto que intenta eliminar no existe.");
            }
            await _repo.EliminarAsync(id);
        }
    }
}
