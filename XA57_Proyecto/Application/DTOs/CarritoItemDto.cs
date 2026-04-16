namespace XA57_Proyecto.Application.DTOs
{
    public class CarritoItemDto
    {
        public int ProductoId { get; set; }
        public int? ModeloAutobusId { get; set; }
        public int? LineaId { get; set; }
        public string? NombreOperador { get; set; }
        public string? NumeroSerie { get; set; }
        public string? Color { get; set; }
        public string? ColorHex { get; set; }
        public string? Ruta { get; set; }
        public string? NotasEspeciales { get; set; }
        public int Cantidad { get; set; }
    }
}