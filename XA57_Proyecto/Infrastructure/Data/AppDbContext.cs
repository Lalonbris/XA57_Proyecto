using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ModeloAutobus> ModelosAutobus { get; set; }
        public DbSet<Linea> Lineas { get; set; }
        public DbSet<TipoProducto> TiposProducto { get; set; }
    }
}
