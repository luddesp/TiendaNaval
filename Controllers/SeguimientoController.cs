using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaNaval.Data;

namespace TiendaNaval.Controllers;

public class SeguimientoController : Controller
{
    private readonly AppDbContext _db;

    public SeguimientoController(AppDbContext db)
    {
        _db = db;
    }


    // =========================================
    // PANTALLA PARA BUSCAR EL PEDIDO
    // =========================================

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }


    // =========================================
    // BUSCAR PEDIDO
    // =========================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Buscar(
        int numeroPedido,
        string telefono)
    {
        // Validar número de pedido
        if (numeroPedido <= 0)
        {
            ViewBag.Error =
                "Ingresá un número de pedido válido.";

            return View("Index");
        }


        // Validar teléfono
        if (string.IsNullOrWhiteSpace(telefono))
        {
            ViewBag.Error =
                "Ingresá el teléfono utilizado en la compra.";

            return View("Index");
        }


        // Normalizamos el teléfono ingresado
        var telefonoIngresado =
            NormalizarTelefono(telefono);


        // Buscamos el pedido por número
        var pedido = await _db.Pedidos
            .AsNoTracking()
            .Include(p => p.Detalles)
            .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(
                p => p.Id == numeroPedido
            );


        /*
         * No informamos si el número de pedido existe
         * y solamente falló el teléfono.
         *
         * De esta manera no exponemos información
         * innecesaria sobre otros pedidos.
         */

        if (pedido == null ||
            NormalizarTelefono(pedido.Telefono)
                != telefonoIngresado)
        {
            ViewBag.Error =
                "No encontramos un pedido con esos datos. " +
                "Revisá el número de pedido y el teléfono ingresado.";

            return View("Index");
        }


        // Pedido encontrado
        return View(
            "Detalle",
            pedido
        );
    }


    // =========================================
    // NORMALIZAR TELÉFONO
    // =========================================

    private static string NormalizarTelefono(
        string telefono)
    {
        if (string.IsNullOrWhiteSpace(telefono))
            return "";

        /*
         * Dejamos únicamente números.
         *
         * Ejemplo:
         *
         * 291 412-3456
         * 2914123456
         *
         * se consideran el mismo teléfono.
         */

        return new string(
            telefono
                .Where(char.IsDigit)
                .ToArray()
        );
    }
}