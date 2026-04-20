using System.Text.RegularExpressions;
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
        private static readonly Regex _regexPermitidos = new(@"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑüÜ\s\-\.\,\/\#]*$");


        public PedidoService(IPedidoRepository pedidoRepo, IProductoRepository productoRepo, ITipoProductoRepository tipoProductoRepo)
        {
            _pedidoRepo = pedidoRepo;
            _productoRepo = productoRepo;
            _tipoProductoRepo = tipoProductoRepo;
        }

        private void ValidarCaracteres(string? valor, string campo)
        {
            if (!string.IsNullOrEmpty(valor) && !_regexPermitidos.IsMatch(valor))
            {
                throw new ValidationException($"{campo} contiene caracteres no permitidos.");
            }
        }

        public async Task<PedidoResultDto> AgregarAsync(CarritoItemDto item)
        {
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

            ValidarCaracteres(item.NombreOperador, "NombreOperador");
            ValidarCaracteres(item.NumeroSerie, "NumeroEconomico");
            ValidarCaracteres(item.Ruta, "Ruta");
            ValidarCaracteres(item.NotasEspeciales, "NotasEspeciales");

            var pedido = new Pedido
            {
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
                Estado          = "Recibido",
                FechaCreacion   = DateTime.UtcNow
            };

            await _pedidoRepo.AgregarAsync(pedido);

            return new PedidoResultDto
            {
                Mensaje   = "Producto agregado al carrito.",
                PedidoId  = pedido.Id
            };
        }

        public Task<List<Pedido>> ObtenerCarritoAsync() => _pedidoRepo.ObtenerConProductosAsync();

        public Task EliminarItemAsync(int id) => _pedidoRepo.EliminarAsync(id);

        public Task<int> ContarItemsAsync() => _pedidoRepo.ContarAsync();
    }
}
