using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace XA57_Proyecto.Domain.Entities
{
    [Table("carritos")]
    public class Carrito
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<ItemCarrito> Items { get; set; } = new List<ItemCarrito>();
    }
}