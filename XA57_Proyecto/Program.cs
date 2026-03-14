using Microsoft.EntityFrameworkCore;
using XA57_Proyecto.Application.Interfaces;
using XA57_Proyecto.Application.Services;
using XA57_Proyecto.Infrastructure.Data;
using XA57_Proyecto.Infrastructure.Repositories;
using XA57_Proyecto.Infrastructure.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Infrastructure
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<ILineaRepository, LineaRepository>();
builder.Services.AddScoped<IModeloAutobusRepository, ModeloAutobusRepository>();

// Application
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<ILineaService, LineaService>();
builder.Services.AddScoped<IModeloAutobusService, ModeloAutobusService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
