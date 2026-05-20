using XA57_Proyecto.Application.Exceptions;
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

        public Task<List<ModeloAutobus>> ObtenerTodosAsync() => _repo.ObtenerTodosAsync();

        public Task<List<ModeloAutobus>> ObtenerActivosAsync() => _repo.ObtenerActivosAsync();

        public Task<ModeloAutobus?> ObtenerPorIdAsync(int id) => _repo.ObtenerPorIdAsync(id);

        public async Task<ModeloAutobus> AgregarAsync(ModeloAutobus modelo)
        {
            if (string.IsNullOrWhiteSpace(modelo.Nombre))
                throw new ValidationException("El nombre del modelo no puede estar vacío.");
            
            if (string.IsNullOrWhiteSpace(modelo.Fabricante))
                throw new ValidationException("El fabricante del modelo no puede estar vacío.");

            return await _repo.AgregarAsync(modelo);
        }

        public async Task ActualizarAsync(ModeloAutobus modelo)
        {
            var existente = await _repo.ObtenerPorIdAsync(modelo.Id);
            if (existente == null)
                throw new ValidationException("El modelo que intenta actualizar no existe.");

            if (string.IsNullOrWhiteSpace(modelo.Nombre))
                throw new ValidationException("El nombre del modelo no puede estar vacío.");
            
            if (string.IsNullOrWhiteSpace(modelo.Fabricante))
                throw new ValidationException("El fabricante del modelo no puede estar vacío.");

            await _repo.ActualizarAsync(modelo);
        }

        public async Task EliminarAsync(int id)
        {
            var modelo = await _repo.ObtenerPorIdAsync(id);
            if (modelo == null)
                throw new ValidationException("El modelo que intenta eliminar no existe.");

            await _repo.EliminarAsync(id);
        }
    }
}
