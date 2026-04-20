using XA57_Proyecto.Application.Interfaces;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Infrastructure.Repositories.Interfaces;

namespace XA57_Proyecto.Application.Services
{
    public class LineaService : ILineaService
    {
        private readonly ILineaRepository _repo;

        public LineaService(ILineaRepository repo)
        {
            _repo = repo;
        }

        public Task<List<Linea>> ObtenerTodosAsync() => _repo.ObtenerTodosAsync();

        public Task<List<Linea>> ObtenerActivasAsync() => _repo.ObtenerActivasAsync();

        public Task<Linea?> ObtenerPorIdAsync(int id) => _repo.ObtenerPorIdAsync(id);

        public Task<Linea> AgregarAsync(Linea linea) => _repo.AgregarAsync(linea);

        public Task ActualizarAsync(Linea linea) => _repo.ActualizarAsync(linea);

        public Task EliminarAsync(int id) => _repo.EliminarAsync(id);
    }
}
