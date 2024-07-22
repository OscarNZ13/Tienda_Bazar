﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tienda_Bazar.Models;

namespace Tienda_Bazar.Controllers
{
    public class ProductosController : Controller
    {
        private readonly BazarLibreriaContext _context;

        public ProductosController(BazarLibreriaContext context)
        {
            _context = context;
        }

        // GET: Productos
        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos
                .Include(p => p.ImagenesProductos)
                .ToListAsync();
            return View(productos);
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.CodigoProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodigoProducto,NombreProducto,Precio,DisponibilidadInventario,Estado")] Producto producto, List<IFormFile> imagenes)
        {
            if (ModelState.IsValid)
            {
                if (imagenes != null && imagenes.Count > 0)
                {
                    foreach (var imagen in imagenes)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await imagen.CopyToAsync(ms);
                            var imagenProducto = new ImagenProducto
                            {
                                Imagen = ms.ToArray(),
                                Producto = producto // Asignar el producto aquí
                            };
                            producto.ImagenesProductos.Add(imagenProducto);
                        }
                    }
                }

                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodigoProducto,NombreProducto,Precio,DisponibilidadInventario,Estado")] Producto producto, List<IFormFile> nuevasImagenes)
        {
            if (id != producto.CodigoProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var productoExistente = await _context.Productos
                        .Include(p => p.ImagenesProductos)
                        .FirstOrDefaultAsync(p => p.CodigoProducto == id);

                    if (productoExistente != null)
                    {
                        productoExistente.NombreProducto = producto.NombreProducto;
                        productoExistente.Precio = producto.Precio;
                        productoExistente.DisponibilidadInventario = producto.DisponibilidadInventario;
                        productoExistente.Estado = producto.Estado;

                        if (nuevasImagenes != null && nuevasImagenes.Count > 0)
                        {
                            foreach (var imagen in nuevasImagenes)
                            {
                                using (var ms = new MemoryStream())
                                {
                                    await imagen.CopyToAsync(ms);
                                    var imagenProducto = new ImagenProducto
                                    {
                                        Imagen = ms.ToArray(),
                                        Producto = productoExistente // Asignar el producto aquí
                                    };
                                    productoExistente.ImagenesProductos.Add(imagenProducto);
                                }
                            }
                        }

                        _context.Update(productoExistente);
                    }
                    else
                    {
                        _context.Update(producto);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.CodigoProducto))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }


        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.CodigoProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.CodigoProducto == id);
        }
    }
}
