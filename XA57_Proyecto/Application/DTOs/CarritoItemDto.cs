using System.ComponentModel.DataAnnotations;

namespace XA57_Proyecto.Application.DTOs
{
    public class CarritoItemDto
    {
        [Required(ErrorMessage = "El producto es obligatorio.")]
        public int ProductoId { get; set; }

        public int? ModeloAutobusId { get; set; }
        public int? LineaId { get; set; }

        [StringLength(100, ErrorMessage = "El nombre del operador no puede exceder los 100 caracteres.")]
        public string? NombreOperador { get; set; }

        [StringLength(100, ErrorMessage = "El número de serie no puede exceder los 100 caracteres.")]
        public string? NumeroSerie { get; set; }

        [StringLength(100, ErrorMessage = "El color no puede exceder los 100 caracteres.")]
        public string? Color { get; set; }

        [StringLength(7, ErrorMessage = "El color hexadecimal debe tener formato válido (ej. #FF0000).")]
        public string? ColorHex { get; set; }

        [StringLength(200, ErrorMessage = "La ruta no puede exceder los 200 caracteres.")]
        public string? Ruta { get; set; }

        [StringLength(500, ErrorMessage = "Las notas especiales no pueden exceder los 500 caracteres.")]
        public string? NotasEspeciales { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, 100, ErrorMessage = "La cantidad debe estar entre 1 y 100.")]
        public int Cantidad { get; set; }
    }
}
