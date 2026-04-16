using XA57_Proyecto.Application.Interfaces;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Domain.Interfaces;

namespace XA57_Proyecto.Application.Services
{
    public class ModeloAutobusService : IModeloAutobusService
    {
        private readonly IModeloAutobusRepository _repo;

        public ModeloAutobusService(IModeloAutobusRepository repo)
        {
            _repo = repo;
        }

        public Task<IReadOnlyList<ModeloAutobus>> GetAllActiveAsync() => _repo.GetAllActiveAsync();

        public Task<ModeloAutobus?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
    }
}