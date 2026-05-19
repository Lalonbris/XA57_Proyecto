using XA57_Proyecto.Application.Exceptions;
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

        public async Task<Linea> AgregarAsync(Linea linea)
        {
            ValidarLinea(linea);
            return await _repo.AgregarAsync(linea);
        }

        public async Task ActualizarAsync(Linea linea)
        {
            var existente = await _repo.ObtenerPorIdAsync(linea.Id);
            if (existente == null)
            {
                throw new ValidationException("La línea que intenta actualizar no existe.");
            }
            ValidarLinea(linea);
            await _repo.ActualizarAsync(linea);
        }

        public async Task EliminarAsync(int id)
        {
            var linea = await _repo.ObtenerPorIdAsync(id);
            if (linea == null)
            {
                throw new ValidationException("La línea que intenta eliminar no existe.");
            }
            await _repo.EliminarAsync(id);
        }

        private static void ValidarLinea(Linea linea)
        {
            if (string.IsNullOrWhiteSpace(linea.Nombre))
            {
                throw new ValidationException("El nombre de la línea no puede estar vacío.");
            }

            if (linea.Nombre.Length > 100)
            {
                throw new ValidationException("El nombre de la línea no puede exceder los 100 caracteres.");
            }

            if (!string.IsNullOrEmpty(linea.NombreOperador) && linea.NombreOperador.Length > 100)
            {
                throw new ValidationException("El nombre del operador no puede exceder los 100 caracteres.");
            }

            if (!string.IsNullOrEmpty(linea.LogoUrl) && linea.LogoUrl.Length > 500)
            {
                throw new ValidationException("La URL del logo no puede exceder los 500 caracteres.");
            }
        }
    }
}
