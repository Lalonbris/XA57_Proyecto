using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XA57_Proyecto.Domain.Entities
{
    [Table("Pedidos")]
    public class Pedido
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("producto_id")]
        public int ProductoId { get; set; }

        [Column("modelo_autobus_id")]
        public int? ModeloAutobusId { get; set; }

        [Column("linea_id")]
        public int? LineaId { get; set; }

        // Campos de personalización (almacenados directamente en Pedido para mostrar en carrito)
        [Column("nombre_operador")]
        public string? NombreOperador { get; set; }

        [Column("numero_economico")]
        public string? NumeroEconomico { get; set; }

        [Column("color")]
        public string? Color { get; set; }

        [Column("color_hex")]
        public string? ColorHex { get; set; }

        [Column("ruta")]
        public string? Ruta { get; set; }

        [Column("notas_especiales")]
        public string? NotasEspeciales { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }

        [Column("estado")]
        public string Estado { get; set; } = "Recibido";

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Producto? Producto { get; set; }
        public virtual ModeloAutobus? ModeloAutobus { get; set; }
        public virtual Linea? Linea { get; set; }
        // Personalizaciones will be handled via a separate entity (e.g., PedidoPersonalizacion) if needed
        // For simplicity, we keep the denormalized fields as per spec; personalization options are defined via PersonalizacionProducto
        // and linked to Pedido via a join table if we need to track which options were selected.
        // However spec shows Pedido has these fields directly, so we keep them.
    }
}