using System.ComponentModel.DataAnnotations;

namespace TiendaNaval.Models;

public class Categoria
{
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; } = "";

    public List<Producto> Productos { get; set; } = [];
}


// =========================
// PRODUCTO
// =========================

public class Producto
{
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; } = "";

    public string Descripcion { get; set; } = "";

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public string? ImagenUrl { get; set; }

    public List<ProductoImagen> Imagenes { get; set; } = new();

    public bool Destacado { get; set; }

    public int CategoriaId { get; set; }

    public Categoria? Categoria { get; set; }


    // =========================
    // OPCIONES DEL PRODUCTO
    // =========================

    public bool UsaJerarquia { get; set; }

    public bool UsaEspecialidad { get; set; }

    public bool UsaTipoTrasero { get; set; }

    public bool UsaInicial { get; set; }

    public bool UsaApellido { get; set; }

    public bool UsaNombre { get; set; }

    public bool TieneJerarquiaAspirantes { get; set; }

    public string? JerarquiaAspirantes { get; set; }


    // =========================
    // COLECCIÓN
    // =========================

    public string TipoColeccion { get; set; } = "General";
}


// =========================
// IMÁGENES DEL PRODUCTO
// =========================

public class ProductoImagen
{
    public int Id { get; set; }

    public int ProductoId { get; set; }

    public Producto? Producto { get; set; }

    [Required]
    public string ImagenUrl { get; set; } = "";

    public int Orden { get; set; }
}


// =========================
// ADMINISTRADOR
// =========================

public class Administrador
{
    public int Id { get; set; }

    [Required]
    public string Usuario { get; set; } = "";

    [Required]
    public string PasswordHash { get; set; } = "";
}


// =========================
// PEDIDO
// =========================

public class Pedido
{
    public int Id { get; set; }

    [Required]
    public string NombreCliente { get; set; } = "";

    [Required]
    public string Telefono { get; set; } = "";

    public string Direccion { get; set; } = "";

    public string Localidad { get; set; } = "";

    public string? Observaciones { get; set; }


    // =========================
    // ENVÍO
    // =========================

    [Required(ErrorMessage = "Seleccioná un método de envío.")]
    public string MetodoEnvio { get; set; } = "";

    public string? Piso { get; set; }

    public string? Departamento { get; set; }

    [Required(ErrorMessage = "Ingresá el barrio.")]
    public string Barrio { get; set; } = "";


    // =========================
    // DATOS DEL PEDIDO
    // =========================

    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    public decimal Total { get; set; }

    public string Estado { get; set; } = "Pendiente";

    public string MetodoPago { get; set; } =
        "Transferencia bancaria";

    public List<DetallePedido> Detalles { get; set; } = [];
}


// =========================
// DETALLE PEDIDO
// =========================

public class DetallePedido
{
    public int Id { get; set; }

    public int PedidoId { get; set; }

    public Pedido? Pedido { get; set; }

    public int ProductoId { get; set; }

    public Producto? Producto { get; set; }


    // =========================
    // PRODUCTO
    // =========================

    public string NombreProducto { get; set; } = "";

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }


    // =========================
    // OPCIONES ELEGIDAS
    // =========================

    public string? Color { get; set; }

    public string? Especialidad { get; set; }

    public string? Jerarquia { get; set; }

    public string? JerarquiaAspirantes { get; set; }

    public string? ConGanchito { get; set; }

    public string? ConNombre { get; set; }

    public string? NombreApellido { get; set; }

    public string? Inicial { get; set; }

    public string? Apellido { get; set; }

    [StringLength(
        12,
        ErrorMessage = "El nombre puede tener como máximo 12 caracteres."
    )]
    public string? NombrePersonalizado { get; set; }


    // =========================
    // SUBTOTAL
    // =========================

    public decimal Subtotal =>
        Cantidad * PrecioUnitario;
}


// =========================
// CARRITO
// =========================

public class CarritoItem
{
    public int ProductoId { get; set; }

    public string Nombre { get; set; } = "";

    public decimal Precio { get; set; }

    public int Cantidad { get; set; }

    public string? ImagenUrl { get; set; }


    // =========================
    // OPCIONES ELEGIDAS
    // =========================

    public string? Color { get; set; }

    public string? Especialidad { get; set; }

    public string? Jerarquia { get; set; }

    public string? JerarquiaAspirantes { get; set; }

    public string? ConGanchito { get; set; }

    public string? ConNombre { get; set; }

    public string? NombreApellido { get; set; }

    public string? Inicial { get; set; }

    public string? Apellido { get; set; }

    [StringLength(
        12,
        ErrorMessage = "El nombre puede tener como máximo 12 caracteres."
    )]
    public string? NombrePersonalizado { get; set; }


    // =========================
    // SUBTOTAL
    // =========================

    public decimal Subtotal =>
        Precio * Cantidad;
}