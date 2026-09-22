using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TiendaNaval.Data;
using TiendaNaval.Models;

namespace TiendaNaval.Controllers;

public class CarritoController : Controller
{
    private readonly AppDbContext db;

    private const string Key = "carrito";

    public CarritoController(AppDbContext db)
    {
        this.db = db;
    }


    // =========================
    // OBTENER CARRITO
    // =========================

    private List<CarritoItem> Get()
    {
        return JsonSerializer.Deserialize<List<CarritoItem>>(
                   HttpContext.Session.GetString(Key) ?? "[]"
               ) ?? new List<CarritoItem>();
    }


    // =========================
    // GUARDAR CARRITO
    // =========================

    private void Save(List<CarritoItem> carrito)
    {
        HttpContext.Session.SetString(
            Key,
            JsonSerializer.Serialize(carrito)
        );
    }


    // =========================
    // CARRITO NORMAL
    // =========================

    public IActionResult Index()
    {
        return View(Get());
    }


    // =========================
    // AGREGAR NORMAL
    // =========================

    [HttpPost]
    public async Task<IActionResult> Agregar(
        int id,
        int cantidad = 1,
        string? color = null,
        string? especialidad = null,
        string? jerarquia = null,
        string? jerarquiaAspirantes = null,
        string? conGanchito = null,
        string? conNombre = null,
        string? nombreApellido = null,
        string? inicial = null,
        string? apellido = null)
    {
        var producto = await db.Productos.FindAsync(id);

        if (producto == null)
            return NotFound();

        var carrito = Get();

        cantidad = Math.Max(1, cantidad);


        // Buscamos un item con el mismo producto
        // Y LAS MISMAS OPCIONES
        var item = carrito.FirstOrDefault(x =>
            x.ProductoId == id &&
            x.Color == color &&
            x.Especialidad == especialidad &&
            x.Jerarquia == jerarquia &&
            x.JerarquiaAspirantes == jerarquiaAspirantes &&
            x.ConGanchito == conGanchito &&
            x.ConNombre == conNombre &&
            x.NombreApellido == nombreApellido &&
            x.Inicial == inicial &&
            x.Apellido == apellido
        );


        if (item == null)
        {
            carrito.Add(new CarritoItem
            {
                ProductoId = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Cantidad = cantidad,
                ImagenUrl = producto.ImagenUrl,

                Color = color,
                Especialidad = especialidad,
                Jerarquia = jerarquia,
                JerarquiaAspirantes = jerarquiaAspirantes,
                ConGanchito = conGanchito,
                ConNombre = conNombre,
                NombreApellido = nombreApellido,
                Inicial = inicial,
                Apellido = apellido
            });
        }
        else
        {
            item.Cantidad += cantidad;
        }


        Save(carrito);

        return RedirectToAction("Index");
    }


    // =========================
    // AGREGAR CON AJAX
    // =========================

    [HttpPost]
    public async Task<IActionResult> AgregarAjax(
        int id,
        int cantidad = 1,
        string? color = null,
        string? especialidad = null,
        string? jerarquia = null,
        string? jerarquiaAspirantes = null,
        string? conGanchito = null,
        string? conNombre = null,
        string? nombreApellido = null,
        string? inicial = null,
        string? apellido = null)
    {
        var producto = await db.Productos.FindAsync(id);

        if (producto == null)
            return NotFound();

        var carrito = Get();

        cantidad = Math.Max(1, cantidad);


        // IMPORTANTE:
        // mismo producto + mismas opciones
        var item = carrito.FirstOrDefault(x =>
            x.ProductoId == id &&
            x.Color == color &&
            x.Especialidad == especialidad &&
            x.Jerarquia == jerarquia &&
            x.JerarquiaAspirantes == jerarquiaAspirantes &&
            x.ConGanchito == conGanchito &&
            x.ConNombre == conNombre &&
            x.NombreApellido == nombreApellido &&
            x.Inicial == inicial &&
            x.Apellido == apellido
        );


        if (item == null)
        {
            carrito.Add(new CarritoItem
            {
                ProductoId = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Cantidad = cantidad,
                ImagenUrl = producto.ImagenUrl,

                Color = color,
                Especialidad = especialidad,
                Jerarquia = jerarquia,
                JerarquiaAspirantes = jerarquiaAspirantes,
                ConGanchito = conGanchito,
                ConNombre = conNombre,
                NombreApellido = nombreApellido,
                Inicial = inicial,
                Apellido = apellido
            });
        }
        else
        {
            item.Cantidad += cantidad;
        }


        Save(carrito);


        return Ok(new
        {
            success = true,
            cantidad = carrito.Sum(x => x.Cantidad),
            total = carrito.Sum(x => x.Subtotal)
        });
    }


    // =========================
    // DRAWER
    // =========================

    [HttpGet]
    public IActionResult Drawer()
    {
        return PartialView(
            "~/Views/Shared/_CarritoDrawer.cshtml",
            Get()
        );
    }


    // =========================
    // ACTUALIZAR CANTIDAD AJAX
    // =========================

    [HttpPost]
    public IActionResult ActualizarAjax(
        int id,
        int cantidad)
    {
        var carrito = Get();

        var item = carrito.FirstOrDefault(
            x => x.ProductoId == id
        );

        if (item == null)
        {
            return Ok(new
            {
                success = true
            });
        }

        if (cantidad <= 0)
        {
            carrito.Remove(item);
        }
        else
        {
            item.Cantidad = cantidad;
        }

        Save(carrito);

        return Ok(new
        {
            success = true,
            cantidad = carrito.Sum(x => x.Cantidad),
            total = carrito.Sum(x => x.Subtotal)
        });
    }


    // =========================
    // QUITAR NORMAL
    // =========================

    [HttpPost]
    public IActionResult Quitar(int id)
    {
        var carrito = Get();

        carrito.RemoveAll(
            x => x.ProductoId == id
        );

        Save(carrito);

        return RedirectToAction("Index");
    }


    // =========================
    // QUITAR DESDE DRAWER
    // =========================

    [HttpPost]
    public IActionResult QuitarAjax(int id)
    {
        var carrito = Get();

        carrito.RemoveAll(
            x => x.ProductoId == id
        );

        Save(carrito);

        return Ok(new
        {
            success = true,
            cantidad = carrito.Sum(x => x.Cantidad),
            total = carrito.Sum(x => x.Subtotal)
        });
    }


    // =========================
    // CANTIDAD DEL CARRITO
    // =========================

    [HttpGet]
    public IActionResult Cantidad()
    {
        var carrito = Get();

        return Ok(new
        {
            cantidad = carrito.Sum(x => x.Cantidad)
        });
    }
}