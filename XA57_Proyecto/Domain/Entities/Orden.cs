using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace XA57_Proyecto.Domain.Entities
{
    [Table("ordenes")]
    public class Orden
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [Column("estado")]
        public string Estado { get; set; } = "Recibido";

        // Navigation properties
        public virtual ICollection<ItemOrden> Items { get; set; } = new List<ItemOrden>();
    }
}