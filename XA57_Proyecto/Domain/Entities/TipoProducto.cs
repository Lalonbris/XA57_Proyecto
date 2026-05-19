using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XA57_Proyecto.Domain.Entities
{
    [Table("tipos_producto")]
    public class TipoProducto
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del tipo de producto es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Range(1, 100, ErrorMessage = "El máximo de caracteres debe estar entre 1 y 100.")]
        [Column("max_caracteres")]
        public int MaxCaracteres { get; set; } = 20;

        [Column("permite_nombre")]
        public bool PermiteNombre { get; set; } = true;

        [Column("permite_numero_economico")]
        public bool PermiteNumeroEconomico { get; set; } = true;

        [Column("permite_ruta")]
        public bool PermiteRuta { get; set; } = true;
    }
}
