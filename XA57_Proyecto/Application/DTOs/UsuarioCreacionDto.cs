using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Application.Dtos
{
    public class UsuarioCreacionDto
    {
        public ApplicationUser Usuario { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }
    }
}