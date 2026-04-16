using XA57_Proyecto.Application.Interfaces;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Domain.Interfaces;

namespace XA57_Proyecto.Application.Services
{
    public class LineaService : ILineaService
    {
        private readonly ILineaRepository _repo;

        public LineaService(ILineaRepository repo)
        {
            _repo = repo;
        }

        public Task<IReadOnlyList<Linea>> GetAllActiveAsync() => _repo.GetAllActiveAsync();

        public Task<Linea?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
    }
}