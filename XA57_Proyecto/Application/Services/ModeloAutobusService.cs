using XA57_Proyecto.Application.Interfaces;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Infrastructure.Repositories.Interfaces;

namespace XA57_Proyecto.Application.Services
{
    public class ModeloAutobusService : IModeloAutobusService
    {
        private readonly IModeloAutobusRepository _repo;

        public ModeloAutobusService(IModeloAutobusRepository repo)
        {
            _repo = repo;
        }

        public Task<List<ModeloAutobus>> ObtenerActivosAsync() => _repo.ObtenerActivosAsync();

        public Task<ModeloAutobus?> ObtenerPorIdAsync(int id) => _repo.ObtenerPorIdAsync(id);
    }
}
