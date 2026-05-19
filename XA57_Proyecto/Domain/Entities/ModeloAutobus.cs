using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XA57_Proyecto.Domain.Entities
{
    [Table("modelos_autobus")]
    public class ModeloAutobus
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del modelo es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "El nombre del fabricante no puede exceder los 100 caracteres.")]
        [Column("fabricante")]
        public string? Fabricante { get; set; }

        [Column("activo")]
        public bool Activo { get; set; } = true;
    }
}
