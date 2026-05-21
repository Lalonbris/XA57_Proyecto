using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using XA57_Proyecto.Application.DTOs;
using XA57_Proyecto.Application.Exceptions;
using XA57_Proyecto.Application.Interfaces;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Infrastructure.Repositories.Interfaces;

namespace XA57_Proyecto.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepo;
        private readonly IProductoRepository _productoRepo;
        private readonly ITipoProductoRepository _tipoProductoRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PedidoService(
            IPedidoRepository pedidoRepo, 
            IProductoRepository productoRepo, 
            ITipoProductoRepository tipoProductoRepo,
            IHttpContextAccessor httpContextAccessor)
        {
            _pedidoRepo = pedidoRepo;
            _productoRepo = productoRepo;
            _tipoProductoRepo = tipoProductoRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PedidoResultDto> AgregarAsync(CarritoItemDto item, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ValidationException("El usuario debe estar autenticado para agregar productos al carrito.");
            }
            
            if (item.Cantidad <= 0)
            {
                throw new ValidationException("La cantidad debe ser mayor a cero.");
            }

            var producto = await _productoRepo.ObtenerPorIdAsync(item.ProductoId);
            if (producto == null || !producto.Activo)
            {
                throw new ValidationException("El producto no existe o no está disponible.");
            }

            if (producto.TipoProductoId.HasValue)
            {
                var tipoProducto = await _tipoProductoRepo.ObtenerPorIdAsync(producto.TipoProductoId.Value);
                if (tipoProducto != null)
                {
                     if (tipoProducto.PermiteNombre && (item.NombreOperador?.Length > tipoProducto.MaxCaracteres))
                     {
                         throw new ValidationException($"El nombre del operador no debe exceder los {tipoProducto.MaxCaracteres} caracteres.");
                     }
                     if (tipoProducto.PermiteRuta && (item.Ruta?.Length > tipoProducto.MaxCaracteres))
                     {
                         throw new ValidationException($"La ruta no debe exceder los {tipoProducto.MaxCaracteres} caracteres.");
                     }
                }
            }

            var pedido = new Pedido
            {
                UsuarioId       = userId,
                ProductoId      = item.ProductoId,
                ModeloAutobusId = item.ModeloAutobusId,
                LineaId         = item.LineaId,
                NombreOperador  = item.NombreOperador ?? "",
                NumeroEconomico = item.NumeroSerie ?? "",
                Color           = item.Color,
                ColorHex        = item.ColorHex,
                Ruta            = item.Ruta ?? "",
                NotasEspeciales = item.NotasEspeciales ?? "",
                Cantidad        = item.Cantidad,
                Estado          = "Pedido",
                FechaCreacion   = DateTime.UtcNow
            };

            await _pedidoRepo.AgregarAsync(pedido);

            return new PedidoResultDto
            {
                Mensaje   = "Producto agregado al carrito.",
                PedidoId  = pedido.Id
            };
        }

        public Task<List<Pedido>> ObtenerCarritoAsync(string userId) => _pedidoRepo.ObtenerPorUsuarioIdAsync(userId, "Pedido");

        public Task EliminarItemAsync(int id) => _pedidoRepo.EliminarAsync(id);

        public Task<int> ContarItemsAsync(string userId) => _pedidoRepo.ContarPorUsuarioIdAsync(userId, "Pedido");

        public Task<List<Pedido>> ObtenerTodosAsync() => _pedidoRepo.ObtenerTodosAsync();

        public Task<Pedido?> ObtenerPorIdAsync(int id) => _pedidoRepo.ObtenerPorIdAsync(id);

        public async Task ActualizarEstadoAsync(int id, string estado)
        {
            var pedido = await _pedidoRepo.ObtenerPorIdAsync(id);
            if (pedido == null)
            {
                throw new ValidationException("El pedido no existe.");
            }

            if (estado == "En Producción" && pedido.Estado != "En Producción" && pedido.TiempoProduccionInicio == null)
            {
                pedido.TiempoProduccionInicio = DateTime.UtcNow;
                pedido.TiempoProduccionFin = null;
            }
            else if (estado == "Enviado" && pedido.TiempoProduccionInicio != null && pedido.TiempoProduccionFin == null)
            {
                pedido.TiempoProduccionFin = DateTime.UtcNow;
            }

            pedido.Estado = estado;
            await _pedidoRepo.ActualizarAsync(pedido);
        }
    }
}

