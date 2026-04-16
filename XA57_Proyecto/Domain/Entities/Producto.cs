using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XA57_Proyecto.Domain.Entities
{
    [Table("productos")]
    public class Producto
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Column("descripcion")]
        public string? Descripcion { get; set; }

        [Column("precio")]
        public decimal Precio { get; set; }

        [Column("stock")]
        public int Stock { get; set; }

        [Column("imagen_url")]
        public string? ImagenUrl { get; set; }

        [Column("es_personalizable")]
        public bool EsPersonalizable { get; set; }

        [Column("categoria_id")]
        public int? CategoriaId { get; set; }

        [Column("tipo_producto_id")]
        public int? TipoProductoId { get; set; }

        [Column("activo")]
        public bool Activo { get; set; } = true;

        // Navigation properties
        public virtual Categoria? Categoria { get; set; }
        public virtual TipoProducto? TipoProducto { get; set; }
    }
}