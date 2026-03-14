using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.ViewModels
{
    public class CarritoViewModel
    {
        public List<Pedido> Items { get; set; } = new();
        public decimal Total => Items.Sum(i => (i.Producto?.Precio ?? 0) * i.Cantidad);
    }
}
