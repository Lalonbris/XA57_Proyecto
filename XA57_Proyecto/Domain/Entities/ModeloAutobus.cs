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

        [Required]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Column("fabricante")]
        public string? Fabricante { get; set; }

        [Column("activo")]
        public bool Activo { get; set; } = true;
    }
}
