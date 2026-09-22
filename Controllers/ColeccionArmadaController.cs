using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaNaval.Data;

namespace TiendaNaval.Controllers;

public class ColeccionArmadaController : Controller
{
    private readonly AppDbContext _db;

    public ColeccionArmadaController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var productos = await _db.Productos
            .Include(p => p.Categoria)
            .Where(p => p.TipoColeccion == "Armada")
            .OrderByDescending(p => p.Destacado)
            .ThenByDescending(p => p.Id)
            .ToListAsync();

        return View(productos);
    }
}