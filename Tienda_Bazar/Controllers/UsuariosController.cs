using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tienda_Bazar.Models;

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
        public async Task<IActionResult> Edit(int id, [Bind("CodigoUsuario,NombreUsuario,EstadoUsuarioId,RolId")] Usuario usuario, string? newPassword)
        {
            if (id != usuario.CodigoUsuario)
            {
                return NotFound();
            }

            try
            {
                var usuarioDb = await _context.Usuario.FindAsync(id);
                if (usuarioDb == null)
                {
                    return NotFound();
                }

                usuarioDb.NombreUsuario = usuario.NombreUsuario;
                usuarioDb.EstadoUsuarioId = usuario.EstadoUsuarioId;
                usuarioDb.RolId = usuario.RolId;
                usuarioDb.UltimaConexion = DateTime.Now;

                // Solo actualizar la contraseña si se ingresó una nueva
                if (!string.IsNullOrEmpty(newPassword))
                {
                    usuarioDb.Contrasena = _passwordHasher.HashPassword(usuarioDb, newPassword);
                }

                _context.Update(usuarioDb);
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
        public async Task<IActionResult> Edit_UN(int id, [Bind("CodigoUsuario,NombreUsuario")] Usuario usuario, string? newPassword)
        {
            if (id != usuario.CodigoUsuario)
            {
                return NotFound();
            }

            try
            {
                var usuarioDb = await _context.Usuario.FindAsync(id);
                if (usuarioDb == null)
                {
                    return NotFound();
                }

                // Actualizamos los campos no relacionados con la contraseña
                usuarioDb.NombreUsuario = usuario.NombreUsuario;
                usuarioDb.UltimaConexion = DateTime.Now;

                // Si se ingresó una nueva contraseña, hashearla y guardarla
                if (!string.IsNullOrEmpty(newPassword))
                {
                    usuarioDb.Contrasena = _passwordHasher.HashPassword(usuarioDb, newPassword);
                }

                _context.Update(usuarioDb);
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
