using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using TiendaNaval.Data;
using TiendaNaval.Models;

namespace TiendaNaval.Controllers;

public class CheckoutController : Controller
{
    private readonly AppDbContext db;
    private readonly IConfiguration cfg;

    public CheckoutController(
        AppDbContext db,
        IConfiguration cfg)
    {
        this.db = db;
        this.cfg = cfg;
    }


    // =========================================================
    // OBTENER CARRITO
    // =========================================================

    private List<CarritoItem> Cart()
    {
        return JsonSerializer.Deserialize<List<CarritoItem>>(
                   HttpContext.Session.GetString("carrito") ?? "[]"
               ) ?? [];
    }


    // =========================================================
    // CHECKOUT
    // =========================================================

    public IActionResult Index()
    {
        ViewBag.Alias = cfg["Tienda:Alias"];
        ViewBag.CBU = cfg["Tienda:CBU"];
        ViewBag.Titular = cfg["Tienda:Titular"];

        return View(new Pedido());
    }


    // =========================================================
    // FINALIZAR PEDIDO
    // =========================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Finalizar(Pedido pedido)
    {
        try
        {
            var carrito = Cart();


            // =====================================================
            // VALIDAR CARRITO
            // =====================================================

            if (carrito.Count == 0)
            {
                return RedirectToAction(
                    "Index",
                    "Carrito"
                );
            }


            // =====================================================
            // TOTAL
            // =====================================================

            pedido.Total =
                carrito.Sum(
                    x => x.Subtotal
                );


            // =====================================================
            // DATOS GENERALES DEL PEDIDO
            // =====================================================

            pedido.MetodoPago =
                "Transferencia bancaria";

            pedido.Estado =
                "Pendiente";

            pedido.Fecha =
                DateTime.UtcNow;


            // =====================================================
            // DETALLES DEL PEDIDO
            // =====================================================

            pedido.Detalles = carrito
                .Select(x => new DetallePedido
                {
                    ProductoId = x.ProductoId,

                    NombreProducto = x.Nombre,

                    Cantidad = x.Cantidad,

                    PrecioUnitario = x.Precio,


                    // =============================================
                    // OPCIONES PERSONALIZADAS
                    // =============================================

                    Color = x.Color,

                    Especialidad = x.Especialidad,

                    Jerarquia = x.Jerarquia,

                    JerarquiaAspirantes =
                        x.JerarquiaAspirantes,

                    ConGanchito =
                        x.ConGanchito,

                    ConNombre =
                        x.ConNombre,

                    NombreApellido =
                        x.NombreApellido,

                    Inicial =
                        x.Inicial,

                    Apellido =
                        x.Apellido,

                    NombrePersonalizado =
                        x.NombrePersonalizado

                })
                .ToList();


            // =====================================================
            // GUARDAR PEDIDO
            // =====================================================

            db.Pedidos.Add(
                pedido
            );

            await db.SaveChangesAsync();


            /*
             * IMPORTANTE:
             *
             * Recién después de SaveChangesAsync()
             * tenemos disponible pedido.Id.
             *
             * Ese será el número que el cliente
             * utilizará para seguir su pedido.
             */


            // =====================================================
            // CREAR MENSAJE DE WHATSAPP
            // =====================================================

            var mensaje =
                new StringBuilder();


            // =====================================================
            // ENCABEZADO
            // =====================================================

            mensaje.AppendLine(
                "⚓ VYR · NUEVO PEDIDO"
            );

            mensaje.AppendLine(
                "━━━━━━━━━━━━━━━━━━━━"
            );

            mensaje.AppendLine();

            mensaje.AppendLine(
                $"📦 PEDIDO #{pedido.Id}"
            );

            mensaje.AppendLine(
                $"📅 Fecha: {pedido.Fecha.ToLocalTime():dd/MM/yyyy HH:mm}"
            );

            mensaje.AppendLine();


            // =====================================================
            // CLIENTE
            // =====================================================

            mensaje.AppendLine(
                "👤 CLIENTE"
            );

            mensaje.AppendLine();

            mensaje.AppendLine(
                $"Nombre: {pedido.NombreCliente}"
            );

            mensaje.AppendLine(
                $"Teléfono: {pedido.Telefono}"
            );

            mensaje.AppendLine();


            // =====================================================
            // PRODUCTOS
            // =====================================================

            mensaje.AppendLine(
                "🛍️ PRODUCTOS"
            );

            mensaje.AppendLine();


            foreach (var item in carrito)
            {
                // PRODUCTO

                mensaje.AppendLine(
                    $"• {item.Cantidad} × {item.Nombre}"
                );


                // PRECIO UNITARIO

                mensaje.AppendLine(
                    $"  Precio: ${item.Precio:N0} c/u"
                );


                // =============================================
                // COLOR
                // =============================================

                if (!string.IsNullOrWhiteSpace(
                        item.Color))
                {
                    mensaje.AppendLine(
                        $"  Color: {item.Color}"
                    );
                }


                // =============================================
                // ESPECIALIDAD
                // =============================================

                if (!string.IsNullOrWhiteSpace(
                        item.Especialidad))
                {
                    mensaje.AppendLine(
                        $"  Especialidad: {item.Especialidad}"
                    );
                }


                // =============================================
                // JERARQUÍA
                // =============================================

                if (!string.IsNullOrWhiteSpace(
                        item.Jerarquia))
                {
                    mensaje.AppendLine(
                        $"  Jerarquía: {item.Jerarquia}"
                    );
                }


                // =============================================
                // JERARQUÍA ASPIRANTE
                // =============================================

                if (!string.IsNullOrWhiteSpace(
                        item.JerarquiaAspirantes))
                {
                    mensaje.AppendLine(
                        $"  Jerarquía aspirante: {item.JerarquiaAspirantes}"
                    );
                }


                // =============================================
                // GANCHO
                // =============================================

                if (!string.IsNullOrWhiteSpace(
                        item.ConGanchito))
                {
                    mensaje.AppendLine(
                        $"  Ganchito atrás: {item.ConGanchito}"
                    );
                }


                // =============================================
                // CON NOMBRE
                // =============================================

                if (!string.IsNullOrWhiteSpace(
                        item.ConNombre))
                {
                    mensaje.AppendLine(
                        $"  Con nombre: {item.ConNombre}"
                    );
                }


                // =============================================
                // NOMBRE Y APELLIDO
                // =============================================

                if (!string.IsNullOrWhiteSpace(
                        item.NombreApellido))
                {
                    mensaje.AppendLine(
                        $"  Nombre y apellido: {item.NombreApellido}"
                    );
                }


                // =============================================
                // INICIAL
                // =============================================

                if (!string.IsNullOrWhiteSpace(
                        item.Inicial))
                {
                    mensaje.AppendLine(
                        $"  Inicial: {item.Inicial}"
                    );
                }


                // =============================================
                // APELLIDO
                // =============================================

                if (!string.IsNullOrWhiteSpace(
                        item.Apellido))
                {
                    mensaje.AppendLine(
                        $"  Apellido: {item.Apellido}"
                    );
                }


                // =============================================
                // NOMBRE PERSONALIZADO
                // =============================================

                if (!string.IsNullOrWhiteSpace(
                        item.NombrePersonalizado))
                {
                    mensaje.AppendLine(
                        $"  Nombre personalizado: {item.NombrePersonalizado}"
                    );
                }


                // =============================================
                // SUBTOTAL
                // =============================================

                mensaje.AppendLine(
                    $"  Subtotal: ${item.Subtotal:N0}"
                );

                mensaje.AppendLine();
            }


            // =====================================================
            // TOTAL
            // =====================================================

            mensaje.AppendLine(
                "━━━━━━━━━━━━━━━━━━━━"
            );

            mensaje.AppendLine(
                $"💰 TOTAL: ${pedido.Total:N0}"
            );

            mensaje.AppendLine(
                "━━━━━━━━━━━━━━━━━━━━"
            );

            mensaje.AppendLine();


            // =====================================================
            // ENTREGA
            // =====================================================

            mensaje.AppendLine(
                "🚚 ENTREGA"
            );

            mensaje.AppendLine();

            mensaje.AppendLine(
                $"Método: {pedido.MetodoEnvio}"
            );


            // DIRECCIÓN

            if (!string.IsNullOrWhiteSpace(
                    pedido.Direccion))
            {
                mensaje.AppendLine(
                    $"Dirección: {pedido.Direccion}"
                );
            }


            // PISO

            if (!string.IsNullOrWhiteSpace(
                    pedido.Piso))
            {
                mensaje.AppendLine(
                    $"Piso: {pedido.Piso}"
                );
            }


            // DEPARTAMENTO

            if (!string.IsNullOrWhiteSpace(
                    pedido.Departamento))
            {
                mensaje.AppendLine(
                    $"Departamento: {pedido.Departamento}"
                );
            }


            // BARRIO

            if (!string.IsNullOrWhiteSpace(
                    pedido.Barrio))
            {
                mensaje.AppendLine(
                    $"Barrio: {pedido.Barrio}"
                );
            }


            // LOCALIDAD

            if (!string.IsNullOrWhiteSpace(
                    pedido.Localidad))
            {
                mensaje.AppendLine(
                    $"Localidad: {pedido.Localidad}"
                );
            }


            // OBSERVACIONES

            if (!string.IsNullOrWhiteSpace(
                    pedido.Observaciones))
            {
                mensaje.AppendLine();

                mensaje.AppendLine(
                    $"📝 Observaciones: {pedido.Observaciones}"
                );
            }


            mensaje.AppendLine();


            // =====================================================
            // PAGO
            // =====================================================

            mensaje.AppendLine(
                "💳 PAGO"
            );

            mensaje.AppendLine();

            mensaje.AppendLine(
                "Transferencia bancaria"
            );

            mensaje.AppendLine();


            // =====================================================
            // SEGUIMIENTO
            // =====================================================

            mensaje.AppendLine(
                "🔎 SEGUIMIENTO DEL PEDIDO"
            );

            mensaje.AppendLine();

            mensaje.AppendLine(
                $"Tu número de pedido es: #{pedido.Id}"
            );

            mensaje.AppendLine();

            mensaje.AppendLine(
                "Guardá este número."
            );

            mensaje.AppendLine(
                "Lo vas a necesitar junto con tu teléfono para consultar el estado de tu compra desde “Seguir pedido” en nuestra tienda."
            );

            mensaje.AppendLine();


            // =====================================================
            // DESPEDIDA
            // =====================================================

            mensaje.AppendLine(
                "Gracias por comprar en VyR ⚓"
            );


            // =====================================================
            // NÚMERO DE WHATSAPP
            // =====================================================

            var numero =
                cfg["Tienda:WhatsApp"];


            if (string.IsNullOrWhiteSpace(
                    numero))
            {
                return Content(
                    "El número de WhatsApp no está configurado en appsettings.json."
                );
            }


            // =====================================================
            // LIMPIAR NÚMERO
            // =====================================================

            numero = new string(
                numero
                    .Where(char.IsDigit)
                    .ToArray()
            );


            // =====================================================
            // CODIFICAR MENSAJE
            // =====================================================

            var texto =
                Uri.EscapeDataString(
                    mensaje.ToString()
                );


            // =====================================================
            // VACIAR CARRITO
            // =====================================================

            HttpContext.Session.Remove(
                "carrito"
            );


            // =====================================================
            // ABRIR WHATSAPP
            // =====================================================

            var url =
                $"https://wa.me/{numero}?text={texto}";


            return Redirect(
                url
            );
        }
        catch (Exception ex)
        {
            return Content(
                $"ERROR AL FINALIZAR EL PEDIDO:\n\n" +
                $"{ex.Message}\n\n" +
                $"{ex.InnerException?.Message}"
            );
        }
    }
}