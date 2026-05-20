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

        [Column("imagen_url")]
        public string? ImagenUrl { get; set; }

        [Column("tipo_producto_id")]
        public int? TipoProductoId { get; set; }

        [Column("linea_id")]
        public int? LineaId { get; set; }

        [Column("tamano")]
        public string? Tamano { get; set; }

        [Column("activo")]
        public bool Activo { get; set; } = true;

        public TipoProducto? TipoProducto { get; set; }

        public Linea? Linea { get; set; }
    }
}
