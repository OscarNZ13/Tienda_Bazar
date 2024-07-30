using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tienda_Bazar.Models;

namespace Tienda_Bazar.Controllers
{
    [Authorize]
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

        [HttpPost]
        public async Task<IActionResult> FinalizarCompra()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdClaim);

            // Obtener los elementos del carrito del usuario
            var carritoItems = _appDbContext.CarritoCompras
                                           .Where(c => c.CodigoUsuario == userId)
                                           .Include(c => c.Producto)
                                           .ToList();

            if (!carritoItems.Any())
            {
                // El carrito está vacío, redirigir o mostrar un mensaje de error
                return RedirectToAction("ViewCarrito");
            }

            // Obtener el usuario actual desde la base de datos
            var usuario = await _appDbContext.Usuario.FindAsync(userId);

            if (usuario == null)
            {
                // Manejar el caso en el que el usuario no se encuentra
                return RedirectToAction("ViewCarrito");
            }

            // Crear el pedido
            var pedido = new Pedido
            {
                CodigoUsuario = userId,
                Usuario = usuario,
                FechaPedido = DateTime.Now
            };

            // Agregar el pedido al contexto
            _appDbContext.Pedidos.Add(pedido);
            await _appDbContext.SaveChangesAsync(); // Guardar para obtener el ID del pedido

            // Crear los detalles del pedido
            var detallesPedido = carritoItems.Select(c => new DetallePedido
            {
                CodigoProducto = c.CodigoProducto,
                Cantidad = c.Cantidad,
                PrecioUnitario = c.Producto.Precio,
                Producto = c.Producto,
                Pedido = pedido // Inicializar la propiedad Pedido
            }).ToList();

            // Agregar los detalles del pedido al contexto
            _appDbContext.DetallesPedidos.AddRange(detallesPedido);
            await _appDbContext.SaveChangesAsync();

            // Limpiar el carrito del usuario
            _appDbContext.CarritoCompras.RemoveRange(carritoItems);
            await _appDbContext.SaveChangesAsync();

            // Redirigir a la vista de confirmación o de detalles del pedido
            return RedirectToAction("ViewDetails", "Pedido", new { id = pedido.CodigoPedido });
        }
    }
}
