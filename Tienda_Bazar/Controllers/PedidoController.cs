using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Tienda_Bazar.Models;

namespace Tienda_Bazar.Controllers
{
    public class PedidoController : Controller
    {
        private readonly BazarLibreriaContext _context;

        public PedidoController(BazarLibreriaContext context)
        {
            _context = context;
        }

        // GET: Pedido/ViewDetails/5
        public async Task<IActionResult> ViewDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.DetallesPedidos)
                .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(m => m.CodigoPedido == id);

            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // GET: Pedido/ViewPedidos
        public async Task<IActionResult> ViewPedidos()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.DetallesPedidos)
                .ThenInclude(d => d.Producto)
                .ToListAsync();

            return View(pedidos);
        }
    }
}
