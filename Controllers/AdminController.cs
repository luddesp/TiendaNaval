using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaNaval.Data;
using TiendaNaval.Models;

namespace TiendaNaval.Controllers;

public class AdminController : Controller
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

    public AdminController(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    private bool EstaLogueado()
    {
        return HttpContext.Session.GetString("admin") == "1";
    }

    // =========================
    // LOGIN
    // =========================

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string usuario, string password)
    {
        var admin = await _db.Administradores
            .FirstOrDefaultAsync(x => x.Usuario == usuario);

        if (admin != null &&
            BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash))
        {
            HttpContext.Session.SetString("admin", "1");
            return RedirectToAction("Index");
        }

        ViewBag.Error = "Usuario o contraseña incorrectos";
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    // =========================
    // PRODUCTOS
    // =========================

    public async Task<IActionResult> Index()
    {
        if (!EstaLogueado())
            return RedirectToAction("Login");

        var productos = await _db.Productos
            .Include(p => p.Categoria)
            .OrderByDescending(p => p.Id)
            .ToListAsync();

        return View(productos);
    }

    public async Task<IActionResult> Crear()
    {
        if (!EstaLogueado())
            return RedirectToAction("Login");

        ViewBag.Categorias = await _db.Categorias
            .OrderBy(c => c.Nombre)
            .ToListAsync();

        return View(new Producto());
    }

    // =========================
    // CREAR PRODUCTO
    // HASTA 10 IMÁGENES
    // =========================

    [HttpPost]
    public async Task<IActionResult> Crear(
        Producto producto,
        List<IFormFile> imagenes)
    {
        if (!EstaLogueado())
            return Unauthorized();

        if (imagenes.Count > 10)
        {
            ViewBag.Categorias = await _db.Categorias
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            ModelState.AddModelError(
                "",
                "Podés cargar como máximo 10 imágenes."
            );

            return View(producto);
        }

        _db.Productos.Add(producto);
        await _db.SaveChangesAsync();

        int orden = 1;

        foreach (var imagen in imagenes)
        {
            if (imagen == null || imagen.Length == 0)
                continue;

            var url = await GuardarImagen(imagen);

            var productoImagen = new ProductoImagen
            {
                ProductoId = producto.Id,
                ImagenUrl = url,
                Orden = orden
            };

            _db.ProductoImagenes.Add(productoImagen);

            if (orden == 1)
            {
                producto.ImagenUrl = url;
            }

            orden++;
        }

        await _db.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    // =========================
    // EDITAR PRODUCTO
    // =========================

    public async Task<IActionResult> Editar(int id)
    {
        if (!EstaLogueado())
            return RedirectToAction("Login");

        var producto = await _db.Productos
            .Include(p => p.Imagenes)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (producto == null)
            return NotFound();

        ViewBag.Categorias = await _db.Categorias
            .OrderBy(c => c.Nombre)
            .ToListAsync();

        return View(producto);
    }

    [HttpPost]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(
        Producto producto,
        List<IFormFile>? imagenes)
    {
        if (!EstaLogueado())
            return Unauthorized();

        var existente = await _db.Productos
            .Include(p => p.Imagenes)
            .FirstOrDefaultAsync(p => p.Id == producto.Id);

        if (existente == null)
            return NotFound();

        // =========================
        // DATOS DEL PRODUCTO
        // =========================

        existente.Nombre = producto.Nombre;
        existente.Descripcion = producto.Descripcion;
        existente.Precio = producto.Precio;
        existente.Stock = producto.Stock;
        existente.CategoriaId = producto.CategoriaId;
        existente.Destacado = producto.Destacado;

        existente.UsaJerarquia = producto.UsaJerarquia;
        existente.UsaEspecialidad = producto.UsaEspecialidad;
        existente.UsaTipoTrasero = producto.UsaTipoTrasero;
        existente.UsaInicial = producto.UsaInicial;
        existente.UsaApellido = producto.UsaApellido;
        existente.UsaNombre = producto.UsaNombre;

        existente.TipoColeccion = producto.TipoColeccion;
        existente.TieneJerarquiaAspirantes =
            producto.TieneJerarquiaAspirantes;

        // =========================
        // NUEVAS IMÁGENES
        // =========================

        var nuevasImagenes = imagenes?
            .Where(x => x != null && x.Length > 0)
            .ToList() ?? new List<IFormFile>();

        int cantidadActual = existente.Imagenes.Count;

        if (cantidadActual + nuevasImagenes.Count > 10)
        {
            TempData["ErrorImagenes"] =
                $"El producto puede tener como máximo 10 imágenes. " +
                $"Actualmente tiene {cantidadActual}.";

            return RedirectToAction("Editar", new { id = producto.Id });
        }

        int siguienteOrden = existente.Imagenes.Any()
            ? existente.Imagenes.Max(x => x.Orden) + 1
            : 1;

        foreach (var archivo in nuevasImagenes)
        {
            var url = await GuardarImagen(archivo);

            var nuevaImagen = new ProductoImagen
            {
                ProductoId = existente.Id,
                ImagenUrl = url,
                Orden = siguienteOrden
            };

            _db.ProductoImagenes.Add(nuevaImagen);

            // Si el producto todavía no tiene principal
            if (string.IsNullOrWhiteSpace(existente.ImagenUrl))
            {
                existente.ImagenUrl = url;
            }

            siguienteOrden++;
        }

        await _db.SaveChangesAsync();

        TempData["Exito"] = "Producto actualizado correctamente.";

        return RedirectToAction("Editar", new { id = producto.Id });
    }
    // =========================
    // ELIMINAR PRODUCTO
    // =========================

    [HttpPost]
    public async Task<IActionResult> Eliminar(int id)
    {
        if (!EstaLogueado())
            return Unauthorized();

        var producto = await _db.Productos.FindAsync(id);

        if (producto == null)
            return NotFound();

        _db.Productos.Remove(producto);
        await _db.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    // =========================
    // CATEGORÍAS
    // =========================

    public async Task<IActionResult> Categorias()
    {
        if (!EstaLogueado())
            return RedirectToAction("Login");

        var categorias = await _db.Categorias
            .OrderBy(c => c.Nombre)
            .ToListAsync();

        return View(categorias);
    }

    [HttpPost]
    public async Task<IActionResult> CrearCategoria(string nombre)
    {
        if (!EstaLogueado())
            return Unauthorized();

        if (!string.IsNullOrWhiteSpace(nombre))
        {
            _db.Categorias.Add(new Categoria
            {
                Nombre = nombre.Trim()
            });

            await _db.SaveChangesAsync();
        }

        return RedirectToAction("Categorias");
    }

    [HttpPost]
    public async Task<IActionResult> EliminarCategoria(int id)
    {
        if (!EstaLogueado())
            return Unauthorized();

        var tieneProductos = await _db.Productos
            .AnyAsync(p => p.CategoriaId == id);

        if (tieneProductos)
        {
            TempData["Error"] =
                "No se puede eliminar una categoría que tiene productos.";

            return RedirectToAction("Categorias");
        }

        var categoria = await _db.Categorias.FindAsync(id);

        if (categoria != null)
        {
            _db.Categorias.Remove(categoria);
            await _db.SaveChangesAsync();
        }

        return RedirectToAction("Categorias");
    }

    // =========================
    // PEDIDOS
    // =========================

    public async Task<IActionResult> Pedidos()
    {
        if (!EstaLogueado())
            return RedirectToAction("Login");

        var pedidos = await _db.Pedidos
            .Include(p => p.Detalles)
            .OrderByDescending(p => p.Fecha)
            .ToListAsync();

        return View(pedidos);
    }
// =========================
// DETALLE DEL PEDIDO
// =========================

    public async Task<IActionResult> DetallePedido(int id)
    {
        if (!EstaLogueado())
            return RedirectToAction("Login");

        var pedido = await _db.Pedidos
            .Include(p => p.Detalles)
            .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pedido == null)
            return NotFound();

        return View(pedido);
    }
    [HttpPost]
    public async Task<IActionResult> CambiarEstado(
        int id,
        string estado)
    {
        if (!EstaLogueado())
            return Unauthorized();

        var pedido = await _db.Pedidos.FindAsync(id);

        if (pedido == null)
            return NotFound();

        pedido.Estado = estado;

        await _db.SaveChangesAsync();

        return RedirectToAction("Pedidos");
    }

    // =========================
    // ELIMINAR PEDIDO
    // =========================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarPedido(int id)
    {
        if (!EstaLogueado())
            return Unauthorized();

        var pedido = await _db.Pedidos
            .Include(p => p.Detalles)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pedido == null)
            return NotFound();

        if (pedido.Detalles != null && pedido.Detalles.Any())
        {
            _db.RemoveRange(pedido.Detalles);
        }

        _db.Pedidos.Remove(pedido);

        await _db.SaveChangesAsync();

        return RedirectToAction("Pedidos");
    }

    // =========================
    // IMÁGENES
    // =========================

    private async Task<string> GuardarImagen(IFormFile imagen)
    {
        var carpeta = Path.Combine(
            _env.WebRootPath,
            "images",
            "productos");

        Directory.CreateDirectory(carpeta);

        var nombre =
            $"{Guid.NewGuid()}{Path.GetExtension(imagen.FileName)}";

        var ruta = Path.Combine(carpeta, nombre);

        await using var stream =
            System.IO.File.Create(ruta);

        await imagen.CopyToAsync(stream);

        return $"/images/productos/{nombre}";
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HacerImagenPrincipal(
        int productoId,
        int imagenId)
    {
        if (!EstaLogueado())
            return Unauthorized();

        var producto = await _db.Productos
            .Include(p => p.Imagenes)
            .FirstOrDefaultAsync(p => p.Id == productoId);

        if (producto == null)
            return NotFound();

        var imagen = producto.Imagenes
            .FirstOrDefault(x => x.Id == imagenId);

        if (imagen == null)
            return NotFound();

        // La elegida pasa a ser principal
        producto.ImagenUrl = imagen.ImagenUrl;

        // Ordenamos todas las imágenes:
        // principal = 1
        imagen.Orden = 1;

        var restantes = producto.Imagenes
            .Where(x => x.Id != imagenId)
            .OrderBy(x => x.Orden)
            .ToList();

        int orden = 2;

        foreach (var item in restantes)
        {
            item.Orden = orden;
            orden++;
        }

        await _db.SaveChangesAsync();

        TempData["Exito"] = "Imagen principal actualizada.";

        return RedirectToAction(
            "Editar",
            new { id = productoId }
        );
    }

    private void EliminarArchivoImagen(string? imagenUrl)
    {
        if (string.IsNullOrWhiteSpace(imagenUrl))
            return;

        var nombreArchivo = Path.GetFileName(imagenUrl);

        if (string.IsNullOrWhiteSpace(nombreArchivo))
            return;

        var ruta = Path.Combine(
            _env.WebRootPath,
            "images",
            "productos",
            nombreArchivo
        );

        if (System.IO.File.Exists(ruta))
        {
            System.IO.File.Delete(ruta);
        }
    }
}