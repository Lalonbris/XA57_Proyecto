using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Application.Dtos
{
    public class UsuarioCreacionDto
    {
        public ApplicationUser Usuario { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Rol { get; set; } = default!;
    }
}