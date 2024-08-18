using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tienda_Bazar.Models;
using Tienda_Bazar.Models.ViewModels;

namespace Tienda_Bazar.Controllers
{
    public class AccesoController : Controller
    {
        private readonly BazarLibreriaContext _appDbContext;
        private readonly PasswordHasher<Usuario> _passwordHasher;

        public AccesoController(BazarLibreriaContext appDbContext)
        {
            _appDbContext = appDbContext;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(Usuario usuario)
        {
            usuario.Contrasena = _passwordHasher.HashPassword(usuario, usuario.Contrasena);
            usuario.UltimaConexion = DateTime.Now;
            usuario.EstadoUsuarioId = 1; // Activo por defecto
            usuario.RolId = 2; // Usuario por defecto

            await _appDbContext.Usuario.AddAsync(usuario);
            await _appDbContext.SaveChangesAsync();

            if (usuario.CodigoUsuario != 0)
            {
                return RedirectToAction("Login", "Acceso");
            }
            else
            {
                ViewData["Mensaje"] = "No se pudo crear el usuario";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UsuarioVM usuario)
        {
            var usuarioRecibido = await _appDbContext.Usuario
                .Include(u => u.EstadoUsuario)
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.NombreUsuario == usuario.NombreUsuario);

            if (usuarioRecibido == null || usuarioRecibido.EstadoUsuario.Nombre != "Activo")
            {
                ViewData["Mensaje"] = "Error al encontrar el usuario o el usuario está inactivo";
                return View();
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(usuarioRecibido, usuarioRecibido.Contrasena, usuario.Contrasena);

            if (verificationResult != PasswordVerificationResult.Success)
            {
                ViewData["Mensaje"] = "Contraseña incorrecta";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuarioRecibido.CodigoUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuarioRecibido.NombreUsuario),
                new Claim(ClaimTypes.Role, usuarioRecibido.Rol.Nombre)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var properties = new AuthenticationProperties { AllowRefresh = true };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Bienvenida", "Paginas");
        }
    }
}