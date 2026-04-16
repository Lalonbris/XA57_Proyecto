using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XA57_Proyecto.Domain.Entities
{
    [Table("personalizaciones_producto")]
    public class PersonalizacionProducto
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("tipo_opcion")]
        public string TipoOpcion { get; set; } = string.Empty;

        [Column("valor_opcion")]
        public string ValorOpcion { get; set; } = string.Empty;

        [Column("precio_extra")]
        public decimal PrecioExtra { get; set; }

        // Navigation property
        public virtual ICollection<ItemOrden> ItemsOrden { get; set; } = new List<ItemOrden>();
    }
}