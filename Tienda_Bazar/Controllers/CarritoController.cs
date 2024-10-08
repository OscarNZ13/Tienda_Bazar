﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tienda_Bazar.Models;
using Tienda_Bazar.Models.ViewModels;

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
                                       .Select(c => new CartItemViewModel //Usa el objeto para mostrar todos atributos en una lista
                                       {
                                           CarritoId = c.CodigoCarrito,
                                           ProductoNombre = c.Producto.NombreProducto,
                                           Cantidad = c.Cantidad,
                                           Precio = c.Producto.Precio,
                                           Subtotal = c.Cantidad * c.Producto.Precio,
                                           MaxQuantity = c.Producto.DisponibilidadInventario
                                       })
                                       .ToList();

            var carritoViewModel = new CartViewModel //Asgina el valor de esas variables para los atributos del view model
            {
                Items = carritoItems,
                Total = carritoItems.Sum(i => i.Subtotal)
            };

            return View(carritoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CambiarCantidad(int carritoId, int nuevaCantidad)
        {
            Console.WriteLine($"Carrito ID: {carritoId}, Nueva Cantidad: {nuevaCantidad}");
            var carritoItem = await _appDbContext.CarritoCompras
                                                 .Include(c => c.Producto) //Llama productos para usar el stock
                                                 .FirstOrDefaultAsync(c => c.CodigoCarrito == carritoId);
            if (carritoItem == null || nuevaCantidad < 1 || nuevaCantidad > carritoItem.Producto.DisponibilidadInventario) //Validacion para evitar numeros negativos o pasarse del stock
            {
                return RedirectToAction("ViewCarrito");
            }

            carritoItem.Cantidad = nuevaCantidad;
            _appDbContext.CarritoCompras.Update(carritoItem); 
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("ViewCarrito");
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

            if (!carritoItems.Any()) //Bandera para evitar que complete la compra si el carro esta vacio
            {
                return RedirectToAction("ViewCarrito");
            }

            // Obtener el usuario actual desde la DB
            var usuario = await _appDbContext.Usuario.FindAsync(userId);


            // Crear pedido
            var pedido = new Pedido
            {
                CodigoUsuario = userId,
                Usuario = usuario,
                FechaPedido = DateTime.Now
            };

            _appDbContext.Pedidos.Add(pedido);
            await _appDbContext.SaveChangesAsync(); 

            // Crear detalles del pedido
            var detallesPedido = carritoItems.Select(c => new DetallePedido
            {
                CodigoProducto = c.CodigoProducto,
                Cantidad = c.Cantidad,
                PrecioUnitario = c.Producto.Precio,
                Producto = c.Producto,
                Pedido = pedido
            }).ToList();

            foreach (var item in carritoItems)
            {
                var producto = await _appDbContext.Productos.FindAsync(item.CodigoProducto);
                if (producto != null)
                {
                    // Restar los productos comprados en la DB
                    producto.DisponibilidadInventario -= item.Cantidad;
                    _appDbContext.Productos.Update(producto);
                }
            }

            _appDbContext.DetallesPedidos.AddRange(detallesPedido);
            await _appDbContext.SaveChangesAsync();

            _appDbContext.CarritoCompras.RemoveRange(carritoItems);
            await _appDbContext.SaveChangesAsync();

            // Redirigir a detalles del pedido
            return RedirectToAction("ViewDetails", "Pedido", new { id = pedido.CodigoPedido });
        }

        [HttpPost]
        public async Task<IActionResult> AgregarAlCarrito(int productoId, int cantidad = 1)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdClaim);
            var usuario = await _appDbContext.Usuario.FindAsync(userId);

            var producto = await _appDbContext.Productos.FindAsync(productoId);

            var carritoItem = await _appDbContext.CarritoCompras.FirstOrDefaultAsync(c => c.CodigoUsuario == userId && c.CodigoProducto == productoId);

            if (carritoItem != null)
            {
                carritoItem.Cantidad += cantidad; //Si ya esta sube la cantidad
            }
            else
            {
                carritoItem = new CarritoCompra //Como no esta agrega el producto al carrito
                {
                    CodigoUsuario = userId,
                    CodigoProducto = productoId,
                    Cantidad = cantidad,
                    Producto = producto,
                    Usuario = usuario
                };
                _appDbContext.CarritoCompras.Add(carritoItem);
            }

            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("ViewCarrito", "Carrito");
        }

        [HttpPost]
        public async Task<IActionResult> EliminarDelCarrito(int carritoId)
        {
            var carritoItem = await _appDbContext.CarritoCompras.FirstOrDefaultAsync(c => c.CodigoCarrito == carritoId); //Busca el producto a eliminar en el carrito

            _appDbContext.CarritoCompras.Remove(carritoItem); //Lo elimina
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("ViewCarrito");
        }

    }
}
  