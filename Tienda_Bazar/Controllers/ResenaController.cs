using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tienda_Bazar.Models;

namespace Tienda_Bazar.Controllers
{
    public class ResenaController : Controller
    {

        private readonly BazarLibreriaContext _appDbContext;

        public ResenaController(BazarLibreriaContext context)
        {
            _appDbContext = context;
        }

        // Ver todas las resenas
        public async Task<IActionResult> ViewResenas(int CodigoProducto)
        {
            Console.WriteLine($"Producto ID recibido: {CodigoProducto}");

            var resenas = await _appDbContext.Resenas
                                    .Include(r => r.Usuario)
                                    .Where(r => r.CodigoProducto == CodigoProducto)
                                    .ToListAsync();

            ViewBag.CodigoProducto = CodigoProducto; //Para guardar el id en la vista

            Console.WriteLine($"Producto ID en ViewBag: {ViewBag.CodigoProducto}");
            return View(resenas);
        }

        // Agregar resena
        [HttpPost]
        public async Task<IActionResult> AgregarResena(int CodigoProducto, string descripcion)
        {
            Console.WriteLine($"Producto ID recibido: {CodigoProducto}");

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdClaim);

            var producto = await _appDbContext.Productos.FindAsync(CodigoProducto);
            var usuario = await _appDbContext.Usuario.FindAsync(userId);

            //if (producto == null)
            //{
            //    Console.WriteLine("Producto no encontrado");
            //    return NotFound("Producto no encontrado");
            //}

            var resena = new Resena
            {
                CodigoProducto = CodigoProducto,
                CodigoUsuario = userId,
                DescipcionResena = descripcion,
                FechaResena = DateTime.Now,
                Producto = producto,
                Usuario = usuario
            };

            _appDbContext.Resenas.Add(resena);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("ViewResenas", new { CodigoProducto });
        }

    }
}
