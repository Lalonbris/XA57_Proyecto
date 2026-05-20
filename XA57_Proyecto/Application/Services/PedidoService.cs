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
        private readonly ILineaRepository _lineaRepo;
        private readonly IModeloAutobusRepository _modeloRepo;

        private static readonly Regex _caracteresValidos = new(@"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑüÜ\s\-\.\,\/\#]+$", RegexOptions.Compiled);

        public PedidoService(
            IPedidoRepository pedidoRepo, 
            IProductoRepository productoRepo, 
            ITipoProductoRepository tipoProductoRepo,
            ILineaRepository lineaRepo,
            IModeloAutobusRepository modeloRepo)
        {
            _pedidoRepo = pedidoRepo;
            _productoRepo = productoRepo;
            _tipoProductoRepo = tipoProductoRepo;
            _lineaRepo = lineaRepo;
            _modeloRepo = modeloRepo;
        }

        public async Task<PedidoResultDto> AgregarAsync(CarritoItemDto item)
        {
            if (item.Cantidad <= 0)
                throw new ValidationException("La cantidad debe ser mayor a cero.");

            // RN-02 — Dependencia de Diseño (Modelo + Línea Obligatorios)
            if (!item.ModeloAutobusId.HasValue)
                throw new ValidationException("Debe seleccionar un modelo de autobús.");
            
            if (!item.LineaId.HasValue)
                throw new ValidationException("Debe seleccionar una línea/cromática.");

            var producto = await _productoRepo.ObtenerPorIdAsync(item.ProductoId);
            if (producto == null || !producto.Activo)
                throw new ValidationException("El producto no existe o no está disponible.");

            // Validar existencia de Modelo y Línea
            var modelo = await _modeloRepo.ObtenerPorIdAsync(item.ModeloAutobusId.Value);
            if (modelo == null) throw new ValidationException("El modelo seleccionado no es válido.");

            var linea = await _lineaRepo.ObtenerPorIdAsync(item.LineaId.Value);
            if (linea == null) throw new ValidationException("La línea seleccionada no es válida.");

            if (producto.TipoProductoId.HasValue)
            {
                var tipoProducto = await _tipoProductoRepo.ObtenerPorIdAsync(producto.TipoProductoId.Value);
                if (tipoProducto != null)
                {
                     // RN-03 — Límites de Personalización
                     if (tipoProducto.PermiteNombre && (item.NombreOperador?.Length > tipoProducto.MaxCaracteres))
                         throw new ValidationException($"El nombre del operador no debe exceder los {tipoProducto.MaxCaracteres} caracteres.");
                     
                     if (tipoProducto.PermiteRuta && (item.Ruta?.Length > tipoProducto.MaxCaracteres))
                         throw new ValidationException($"La ruta no debe exceder los {tipoProducto.MaxCaracteres} caracteres.");

                     // RN-04 — Caracteres Permitidos
                     ValidarTexto(item.NombreOperador, "Nombre del operador");
                     ValidarTexto(item.NumeroSerie, "Número económico");
                     ValidarTexto(item.Ruta, "Ruta");
                }
            }

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

        private void ValidarTexto(string? texto, string nombreCampo)
        {
            if (string.IsNullOrEmpty(texto)) return;
            if (!_caracteresValidos.IsMatch(texto))
                throw new ValidationException($"El campo '{nombreCampo}' contiene caracteres no permitidos.");
        }

        public Task<List<Pedido>> ObtenerCarritoAsync() => _pedidoRepo.ObtenerConProductosAsync();

        public Task EliminarItemAsync(int id) => _pedidoRepo.EliminarAsync(id);

        public Task<int> ContarItemsAsync() => _pedidoRepo.ContarAsync();

        public Task<List<Pedido>> ObtenerTodosAsync() => _pedidoRepo.ObtenerTodosAsync();

        public Task<Pedido?> ObtenerPorIdAsync(int id) => _pedidoRepo.ObtenerPorIdAsync(id);

        public async Task ActualizarEstadoAsync(int id, string estado)
        {
            var pedido = await _pedidoRepo.ObtenerPorIdAsync(id);
            if (pedido == null) throw new ValidationException("El pedido no existe.");

            // RN-11 — Flujo de Estado del Pedido
            if (!EsTransicionValida(pedido.Estado, estado))
                throw new ValidationException($"Transición de estado inválida: {pedido.Estado} -> {estado}");

            await _pedidoRepo.ActualizarEstadoAsync(id, estado);
        }

        private static readonly Dictionary<string, string> _transicionesValidas = new()
        {
            ["Recibido"] = "En producción",
            ["En producción"] = "Enviado",
            ["Enviado"] = "Entregado"
        };

        private bool EsTransicionValida(string actual, string nuevo)
        {
            if (actual == nuevo) return true;
            return _transicionesValidas.TryGetValue(actual, out var siguiente) && siguiente == nuevo;
        }
    }
}
