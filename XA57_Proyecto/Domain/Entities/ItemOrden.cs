using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XA57_Proyecto.Domain.Entities
{
    [Table("items_orden")]
    public class ItemOrden
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("orden_id")]
        public int OrdenId { get; set; }

        [Column("producto_id")]
        public int ProductoId { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }

        // Navigation properties
        public virtual Orden Orden { get; set; } = null!;
        public virtual Producto Producto { get; set; } = null!;
        public virtual PersonalizacionProducto? Personalizacion { get; set; }
    }
}