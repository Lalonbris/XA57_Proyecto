using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Application.Interfaces
{
    public interface ILineaService
    {
        Task<List<Linea>> ObtenerTodosAsync();
        Task<List<Linea>> ObtenerActivasAsync();
        Task<Linea?> ObtenerPorIdAsync(int id);
        Task<Linea> AgregarAsync(Linea linea);
        Task ActualizarAsync(Linea linea);
        Task EliminarAsync(int id);
    }
}
