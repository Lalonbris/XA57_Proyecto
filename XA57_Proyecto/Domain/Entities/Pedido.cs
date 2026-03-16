using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XA57_Proyecto.Domain.Entities
{
    [Table("Pedidos")]
    public class Pedido
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("producto_id")]
        public int ProductoId { get; set; }

        [Column("modelo_autobus_id")]
        public int? ModeloAutobusId { get; set; }

        [Column("linea_id")]
        public int? LineaId { get; set; }

        // Campos de personalización (Spec Complementaria)
        [Column("nombre_operador")]
        public string? NombreOperador { get; set; }

        [Column("numero_economico")]
        public string? NumeroEconomico { get; set; }

        [Column("color")]
        public string? Color { get; set; }

        [Column("color_hex")]
        public string? ColorHex { get; set; }

        [Column("ruta")]
        public string? Ruta { get; set; }

        [Column("notas_especiales")]
        public string? NotasEspeciales { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }

        [Column("estado")]
        public string Estado { get; set; } = "Recibido";

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public Producto? Producto { get; set; }
        public ModeloAutobus? ModeloAutobus { get; set; }
        public Linea? Linea { get; set; }
    }
}
