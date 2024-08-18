using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tienda_Bazar.Controllers
{
    public class PaginasController : Controller
    {
        public ActionResult Bienvenida()
        {
            return View();
        }

        // Acciones futuras para Contacto y Redes
        public ActionResult Contacto()
        {
            return View();
        }

        public ActionResult Redes()
        {
            return View();
        }
    }
}
