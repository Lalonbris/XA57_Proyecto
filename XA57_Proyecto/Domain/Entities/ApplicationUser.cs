using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace XA57_Proyecto.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; } = string.Empty;
    }
}
