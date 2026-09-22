using Microsoft.EntityFrameworkCore;
using TiendaNaval.Models;

namespace TiendaNaval.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<Producto> Productos => Set<Producto>();

    public DbSet<Categoria> Categorias => Set<Categoria>();

    public DbSet<Administrador> Administradores => Set<Administrador>();

    public DbSet<Pedido> Pedidos => Set<Pedido>();

    public DbSet<DetallePedido> DetallePedidos => Set<DetallePedido>();

    // Imágenes adicionales de los productos
    public DbSet<ProductoImagen> ProductoImagenes => Set<ProductoImagen>();
}