using Microsoft.AspNetCore.Mvc;
using XA57_Proyecto.Models;
using Microsoft.EntityFrameworkCore;

namespace XA57_Proyecto.Controllers
{
    public class ProductosController : Controller
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null) return NotFound();

            return View(producto);
        }
        public async Task<IActionResult> Catalogo()
        {
            var productos = await _context.Productos.ToListAsync();
            return View(productos);
        }
    }
}