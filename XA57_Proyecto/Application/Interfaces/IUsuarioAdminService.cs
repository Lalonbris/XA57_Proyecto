using Microsoft.AspNetCore.Identity;
using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Application.Interfaces
{
    public interface IUsuarioAdminService
    {
        Task<List<ApplicationUser>> ObtenerTodosAsync();
        Task<ApplicationUser?> ObtenerPorIdAsync(string id);
        Task<ApplicationUser?> ObtenerPorEmailAsync(string email);
        Task<IdentityResult> CrearUsuarioAsync(ApplicationUser user, string password);
        Task<IdentityResult> ActualizarUsuarioAsync(ApplicationUser user);
        Task<IdentityResult> EliminarUsuarioAsync(string id);
        Task<IdentityResult> AgregarRolAsync(string usuarioId, string rol);
        Task<IdentityResult> QuitarRolAsync(string usuarioId, string rol);
        Task<IList<string>> ObtenerRolesAsync(string usuarioId);
        Task<List<string>> ObtenerTodosLosRolesAsync();
    }
}