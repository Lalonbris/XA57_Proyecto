using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XA57_Proyecto.Domain.Entities
{
    [Table("items_carrito")]
    public class ItemCarrito
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("carrito_id")]
        public int CarritoId { get; set; }

        [Column("producto_id")]
        public int ProductoId { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }

        // Navigation properties
        public virtual Carrito Carrito { get; set; } = null!;
        public virtual Producto Producto { get; set; } = null!;
    }
}