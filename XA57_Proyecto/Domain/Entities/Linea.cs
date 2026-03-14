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

        [Required]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Column("color_primario")]
        public string? ColorPrimario { get; set; }

        [Column("color_secundario")]
        public string? ColorSecundario { get; set; }

        [Column("nombre_operador")]
        public string? NombreOperador { get; set; }

        [Column("logo_url")]
        public string? LogoUrl { get; set; }

        [Column("activa")]
        public bool Activa { get; set; } = true;
    }
}
