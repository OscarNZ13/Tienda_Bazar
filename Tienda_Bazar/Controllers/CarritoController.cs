using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Tienda_Bazar.Models;

namespace Tienda_Bazar.Controllers
{
    public class CarritoController : Controller
    {
        private readonly BazarLibreriaContext _appDbContext;

        public CarritoController(BazarLibreriaContext context)
        {
            _appDbContext = context;
        }

        public IActionResult ViewCarrito()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdClaim);
            var carritoItems = _appDbContext.CarritoCompras
                                       .Where(c => c.CodigoUsuario == userId)
                                       .Select(c => new CartItemViewModel
                                       {
                                           ProductoId = c.CodigoProducto,
                                           ProductoNombre = c.Producto.NombreProducto,
                                           Cantidad = c.Cantidad,
                                           Precio = c.Producto.Precio,
                                           Subtotal = c.Cantidad * c.Producto.Precio
                                       })
                                       .ToList();

            var carritoViewModel = new CartViewModel
            {
                Items = carritoItems,
                Total = carritoItems.Sum(i => i.Subtotal)
            };

            return View(carritoViewModel);
        }
    }
}