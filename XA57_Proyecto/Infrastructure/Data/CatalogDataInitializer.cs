using XA57_Proyecto.Domain.Entities;
using XA57_Proyecto.Infrastructure.Data;

namespace XA57_Proyecto.Infrastructure.Data
{
    public static class CatalogDataInitializer
    {
        public static async Task SeedData(AppDbContext context)
        {
            var tiposIds = new List<int>();
            var modelosIds = new List<int>();
            var lineasIds = new List<int>();

            if (!context.TiposProducto.Any())
            {
                var tipos = new[]
                {
                    new TipoProducto { Nombre = "Peluche", MaxCaracteres = 25, PermiteNombre = true, PermiteNumeroEconomico = true, PermiteRuta = true },
                    new TipoProducto { Nombre = "Llavero", MaxCaracteres = 15, PermiteNombre = true, PermiteNumeroEconomico = false, PermiteRuta = false },
                    new TipoProducto { Nombre = "Almohada", MaxCaracteres = 30, PermiteNombre = true, PermiteNumeroEconomico = true, PermiteRuta = true },
                    new TipoProducto { Nombre = "Cinta", MaxCaracteres = 10, PermiteNombre = true, PermiteNumeroEconomico = false, PermiteRuta = false },
                };
                context.TiposProducto.AddRange(tipos);
                await context.SaveChangesAsync();
            }
            tiposIds = context.TiposProducto.Select(t => t.Id).ToList();

            if (!context.ModelosAutobus.Any())
            {
                var modelos = new[]
                {
                    new ModeloAutobus { Nombre = "Irizar i8", Fabricante = "Irizar", Activo = true },
                    new ModeloAutobus { Nombre = "Volvo 9800", Fabricante = "Volvo", Activo = true },
                    new ModeloAutobus { Nombre = "Scania Touring", Fabricante = "Scania", Activo = true },
                    new ModeloAutobus { Nombre = "Mercedes-Benz Travego", Fabricante = "Mercedes-Benz", Activo = true },
                };
                context.ModelosAutobus.AddRange(modelos);
                await context.SaveChangesAsync();
            }
            modelosIds = context.ModelosAutobus.Select(m => m.Id).ToList();

            if (!context.Lineas.Any())
            {
                var lineas = new[]
                {
                    new Linea { Nombre = "ETN", ColorPrimario = "#1e3a5f", ColorSecundario = "#c8a84e", NombreOperador = "ETN Turistar", Activa = true },
                    new Linea { Nombre = "Omnibus de México", ColorPrimario = "#cc0000", ColorSecundario = "#ffffff", NombreOperador = "ODM", Activa = true },
                    new Linea { Nombre = "Primera Plus", ColorPrimario = "#003366", ColorSecundario = "#ffcc00", NombreOperador = "Primera Plus", Activa = true },
                    new Linea { Nombre = "Estrella Roja", ColorPrimario = "#dc143c", ColorSecundario = "#ffd700", NombreOperador = "Estrella Roja", Activa = true },
                    new Linea { Nombre = "Futura", ColorPrimario = "#006400", ColorSecundario = "#ff8c00", NombreOperador = "Futura", Activa = true },
                };
                context.Lineas.AddRange(lineas);
                await context.SaveChangesAsync();
            }

            if (!context.Productos.Any())
            {
                var pelucheId = tiposIds[0];
                var llaveroId = tiposIds[1];

                var productos = new[]
                {
                    new Producto
                    {
                        Nombre = "Busito Irizar i8 — ETN",
                        Descripcion = "Peluche del Irizar i8 con cromática ETN. Suave, detallado y coleccionable.",
                        Precio = 399.00m,
                        ImagenUrl = "/images/products/busito-irizar-etn.png",
                        TipoProductoId = pelucheId,
                        Activo = true
                    },
                    new Producto
                    {
                        Nombre = "Busito Volvo 9800 — Primera Plus",
                        Descripcion = "Peluche Volvo 9800 en azul corporativo con dorado. Ideal para regalo.",
                        Precio = 399.00m,
                        ImagenUrl = "/images/products/busito-volvo-pp.png",
                        TipoProductoId = pelucheId,
                        Activo = true
                    },
                    new Producto
                    {
                        Nombre = "Llavero Scania Touring — ODM",
                        Descripcion = "Llavero metálico del Scania Touring con esmalte rojo ODM.",
                        Precio = 149.00m,
                        ImagenUrl = "/images/products/llavero-scania-odm.png",
                        TipoProductoId = llaveroId,
                        Activo = true
                    },
                    new Producto
                    {
                        Nombre = "Llavero MB Travego — Futura",
                        Descripcion = "Llavero del Mercedes-Benz Travego, verde Futura con detalles naranjas.",
                        Precio = 149.00m,
                        ImagenUrl = "/images/products/llavero-travego-futura.png",
                        TipoProductoId = llaveroId,
                        Activo = true
                    },
                    new Producto
                    {
                        Nombre = "Almohada Irizar i8 — Estrella Roja",
                        Descripcion = "Almohada grande con forma de autobús. Funda lavable, relleno hipoalergénico.",
                        Precio = 299.00m,
                        ImagenUrl = "/images/products/almohada-irizar-er.png",
                        TipoProductoId = tiposIds[2],
                        Activo = true
                    },
                    new Producto
                    {
                        Nombre = "Busito Clásico — Personalizable",
                        Descripcion = "Peluche genérico de autobús. Perfecto para personalizar con cualquier línea.",
                        Precio = 349.00m,
                        ImagenUrl = "/images/products/busito-clasico.png",
                        TipoProductoId = pelucheId,
                        Activo = true
                    },
                };
                context.Productos.AddRange(productos);
                await context.SaveChangesAsync();
            }
        }
    }
}
