using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tienda_Bazar.Models;
using Tienda_Bazar.Models.ViewModels;

namespace Tienda_Bazar.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly BazarLibreriaContext _context;
        private readonly PasswordHasher<Usuario> _passwordHasher;

        public UsuariosController(BazarLibreriaContext appDbContext)
        {
            _context = appDbContext;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var bazarLibreriaContext = _context.Usuario.Include(u => u.EstadoUsuario).Include(u => u.Rol);
            return View(await bazarLibreriaContext.ToListAsync());
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewData["EstadoUsuarioId"] = new SelectList(_context.EstadoUsuarios, "EstadoUsuarioId", "Nombre");
            ViewData["RolId"] = new SelectList(_context.Roles, "RolId", "Nombre");
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        public async Task<IActionResult> Create(Usuario Usuario)
        {

            // Se le setean los valores al usuario
            Usuario.NombreUsuario = Usuario.NombreUsuario;
            Usuario.Contrasena = _passwordHasher.HashPassword(Usuario, Usuario.Contrasena);
            Usuario.UltimaConexion = DateTime.Now;
            Usuario.EstadoUsuarioId = Usuario.EstadoUsuarioId;
            Usuario.RolId = Usuario.RolId;

            //Se envia a la db
            await _context.Usuario.AddAsync(Usuario);
            await _context.SaveChangesAsync();

            // Si se crea el ususario el id seria diferente de 0, por lo cual se habria registrado
            if (Usuario.CodigoUsuario != 0)
            {
                return RedirectToAction("Index", "Usuarios");
            }
            else
            {
                ViewData["Mensaje"] = "No sé pudo crear el usuario";
                return View();
            }
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["EstadoUsuarioId"] = new SelectList(_context.EstadoUsuarios, "EstadoUsuarioId", "Nombre", usuario.EstadoUsuarioId);
            ViewData["RolId"] = new SelectList(_context.Roles, "RolId", "Nombre", usuario.RolId);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodigoUsuario,NombreUsuario,Contrasena,EstadoUsuarioId,RolId")] Usuario usuario)
        {
            if (id != usuario.CodigoUsuario)
            {
                return NotFound();
            }

            try
            {
                usuario.UltimaConexion = DateTime.Now;
                _context.Update(usuario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(usuario.CodigoUsuario))
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

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit_UN(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["EstadoUsuarioId"] = new SelectList(_context.EstadoUsuarios, "EstadoUsuarioId", "Nombre", usuario.EstadoUsuarioId);
            ViewData["RolId"] = new SelectList(_context.Roles, "RolId", "Nombre", usuario.RolId);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit_UN(int id, [Bind("CodigoUsuario,NombreUsuario,Contrasena,EstadoUsuarioId,RolId")] Usuario usuario)
        {
            if (id != usuario.CodigoUsuario)
            {
                return NotFound();
            }

            try
            {
                usuario.UltimaConexion = DateTime.Now;
                _context.Update(usuario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(usuario.CodigoUsuario))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .Include(u => u.EstadoUsuario)
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.CodigoUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuario.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.CodigoUsuario == id);
        }
    }
}
