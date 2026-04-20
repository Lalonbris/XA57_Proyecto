using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using XA57_Proyecto.Application.Interfaces;
using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Infrastructure.Data;

namespace XA57_Proyecto.Application.Services
{
    public class UsuarioAdminService : IUsuarioAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsuarioAdminService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public Task<List<ApplicationUser>> ObtenerTodosAsync() =>
            _userManager.Users.ToListAsync();

        public Task<ApplicationUser?> ObtenerPorIdAsync(string id) =>
            _userManager.FindByIdAsync(id);

        public Task<ApplicationUser?> ObtenerPorEmailAsync(string email) =>
            _userManager.FindByEmailAsync(email);

        public async Task<IdentityResult> CrearUsuarioAsync(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> ActualizarUsuarioAsync(ApplicationUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> EliminarUsuarioAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Usuario no encontrado" });
            }
            return await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> AgregarRolAsync(string usuarioId, string rol)
        {
            var user = await _userManager.FindByIdAsync(usuarioId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Usuario no encontrado" });
            }
            return await _userManager.AddToRoleAsync(user, rol);
        }

        public async Task<IdentityResult> QuitarRolAsync(string usuarioId, string rol)
        {
            var user = await _userManager.FindByIdAsync(usuarioId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Usuario no encontrado" });
            }
            return await _userManager.RemoveFromRoleAsync(user, rol);
        }

        public Task<IList<string>> ObtenerRolesAsync(string usuarioId) =>
            _userManager.GetRolesAsync(_userManager.FindByIdAsync(usuarioId).Result!);

        public Task<List<string>> ObtenerTodosLosRolesAsync() =>
            _roleManager.Roles.Select(r => r.Name!).ToListAsync();
    }
}