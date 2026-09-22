using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaNaval.Data;

namespace TiendaNaval.Controllers;

public class ProductosController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index(int? categoria)
    {
        var q = db.Productos
            .Include(p => p.Categoria)
            .AsQueryable();

        if (categoria.HasValue)
        {
            q = q.Where(p => p.CategoriaId == categoria);
        }

        ViewBag.Categorias = await db.Categorias
            .ToListAsync();

        return View(await q.ToListAsync());
    }


    // =========================
    // BUSCAR PRODUCTOS
    // =========================

    [HttpGet]
    public async Task<IActionResult> Sugerencias(string q)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return Json(Array.Empty<object>());
        }

        q = q.Trim();

        var productos = await db.Productos
            .Where(p => p.Nombre.Contains(q))
            .OrderBy(p => p.Nombre)
            .Take(6)
            .Select(p => new
            {
                id = p.Id,
                nombre = p.Nombre,
                precio = p.Precio,
                imagen = p.ImagenUrl
            })
            .ToListAsync();

        return Json(productos);
    }

    // =========================
    // DETALLE PRODUCTO
    // =========================

    public async Task<IActionResult> Detalle(int id)
    {
        var p = await db.Productos
            .Include(x => x.Categoria)
            .Include(x => x.Imagenes)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (p == null)
        {
            return NotFound();
        }

        p.Imagenes = p.Imagenes
            .OrderBy(x => x.Orden)
            .ToList();

        return View(p);
    }
}