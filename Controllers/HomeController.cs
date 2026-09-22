using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore; using TiendaNaval.Data;
namespace TiendaNaval.Controllers;
public class HomeController(AppDbContext db):Controller { public async Task<IActionResult> Index()=>View(await db.Productos.Include(p=>p.Categoria).OrderByDescending(p=>p.Destacado).Take(8).ToListAsync()); }
