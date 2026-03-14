using XA57_Proyecto.Application.DTOs;
using XA57_Proyecto.Application.Interfaces;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Infrastructure.Repositories.Interfaces;

namespace XA57_Proyecto.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _repo;

        public PedidoService(IPedidoRepository repo)
        {
            _repo = repo;
        }

        public async Task<PedidoResultDto> AgregarAsync(CarritoItemDto item)
        {
            var pedido = new Pedido
            {
                ProductoId      = item.ProductoId,
                ModeloAutobusId = item.ModeloAutobusId,
                LineaId         = item.LineaId,
                NombreOperador  = item.NombreOperador ?? "",
                NumeroEconomico = item.NumeroEconomico ?? "",
                Ruta            = item.Ruta ?? "",
                NotasEspeciales = item.NotasEspeciales ?? "",
                Cantidad        = item.Cantidad > 0 ? item.Cantidad : 1,
                Estado          = "Recibido",
                FechaCreacion   = DateTime.UtcNow
            };

            await _repo.AgregarAsync(pedido);

            return new PedidoResultDto
            {
                Mensaje   = "Producto agregado al carrito.",
                PedidoId  = pedido.Id
            };
        }

        public Task<List<Pedido>> ObtenerCarritoAsync() => _repo.ObtenerConProductosAsync();

        public Task EliminarItemAsync(int id) => _repo.EliminarAsync(id);

        public Task<int> ContarItemsAsync() => _repo.ContarAsync();
    }
}
