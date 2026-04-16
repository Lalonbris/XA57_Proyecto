using XA57_Proyecto.Application.Exceptions;
using XA57_Proyecto.Application.Interfaces;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Domain.Interfaces;

namespace XA57_Proyecto.Application.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repo;
        private readonly ITipoProductoRepository _tipoProductoRepo;

        public ProductoService(IProductoRepository repo, ITipoProductoRepository tipoProductoRepo)
        {
            _repo = repo;
            _tipoProductoRepo = tipoProductoRepo;
        }

        public Task<IReadOnlyList<Producto>> GetAllAsync() => _repo.GetAllAsync();

        public Task<Producto?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public async Task<Producto> AddAsync(Producto producto)
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
                var tipoProducto = await _tipoProductoRepo.GetByIdAsync(producto.TipoProductoId.Value);
                if (tipoProducto == null)
                {
                    throw new ValidationException("El tipo de producto especificado no es válido.");
                }
            }

            return await _repo.AddAsync(producto);
        }

        public async Task UpdateAsync(Producto producto)
        {
            var productoExistente = await _repo.GetByIdAsync(producto.Id);
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
                var tipoProducto = await _tipoProductoRepo.GetByIdAsync(producto.TipoProductoId.Value);
                if (tipoProducto == null)
                {
                    throw new ValidationException("El tipo de producto especificado no es válido.");
                }
            }

            await _repo.UpdateAsync(producto);
        }
    }
}