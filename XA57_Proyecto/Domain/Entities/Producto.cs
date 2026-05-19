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

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        [Column("descripcion")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, 999999.99, ErrorMessage = "El precio debe ser mayor a 0.")]
        [Column("precio")]
        public decimal Precio { get; set; }

        [StringLength(500, ErrorMessage = "La URL de la imagen no puede exceder los 500 caracteres.")]
        [Column("imagen_url")]
        public string? ImagenUrl { get; set; }

        [Column("tipo_producto_id")]
        public int? TipoProductoId { get; set; }

        [Column("activo")]
        public bool Activo { get; set; } = true;

        public TipoProducto? TipoProducto { get; set; }
    }
}
