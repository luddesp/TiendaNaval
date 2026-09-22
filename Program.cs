using Microsoft.EntityFrameworkCore;
using TiendaNaval.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o => { o.IdleTimeout = TimeSpan.FromHours(2); o.Cookie.HttpOnly = true; o.Cookie.IsEssential = true; });
var app = builder.Build();
if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Home/Error");
app.UseStaticFiles(); app.UseRouting(); app.UseSession();
app.MapControllerRoute(name:"default", pattern:"{controller=Home}/{action=Index}/{id?}");
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TiendaNaval.Data.AppDbContext>();

    if (!db.Administradores.Any())
    {
        db.Administradores.Add(new TiendaNaval.Models.Administrador
        {
            Usuario = "violeta",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("2528")
        });

        db.SaveChanges();
    }
}
app.Run();
