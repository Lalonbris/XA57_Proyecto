using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Infrastructure.Repositories.Interfaces
{
    public interface ILineaRepository
    {
        Task<List<Linea>> ObtenerTodosAsync();
        Task<List<Linea>> ObtenerActivasAsync();
        Task<Linea?> ObtenerPorIdAsync(int id);
        Task<Linea> AgregarAsync(Linea linea);
        Task ActualizarAsync(Linea linea);
        Task EliminarAsync(int id);
    }
}
