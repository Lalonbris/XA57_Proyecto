using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XA57_Proyecto.Domain.Entities
{
    [Table("lineas")]
    public class Linea
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la línea es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(7, ErrorMessage = "El color debe tener formato hexadecimal (ej. #FF0000).")]
        [Column("color_primario")]
        public string? ColorPrimario { get; set; }

        [StringLength(7, ErrorMessage = "El color debe tener formato hexadecimal (ej. #FFFFFF).")]
        [Column("color_secundario")]
        public string? ColorSecundario { get; set; }

        [StringLength(100, ErrorMessage = "El nombre del operador no puede exceder los 100 caracteres.")]
        [Column("nombre_operador")]
        public string? NombreOperador { get; set; }

        [StringLength(500, ErrorMessage = "La URL del logo no puede exceder los 500 caracteres.")]
        [Column("logo_url")]
        public string? LogoUrl { get; set; }

        [Column("activa")]
        public bool Activa { get; set; } = true;
    }
}
