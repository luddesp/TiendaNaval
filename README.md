# Tienda Naval

E-commerce ASP.NET Core MVC (.NET 10) con estética premium naval, PostgreSQL, carrito por sesión, transferencia bancaria, pedido por WhatsApp y panel administrador.

## Antes de ejecutar
1. Abrir `appsettings.json` y cambiar contraseña de PostgreSQL, WhatsApp, Alias, CBU y Titular.
2. Crear la base `TiendaNavalDB` en pgAdmin.
3. En terminal: `dotnet restore`
4. Crear las tablas. Opción recomendada: instalar `dotnet-ef`, agregar `Microsoft.EntityFrameworkCore.Design`, crear una migración y ejecutar `dotnet ef database update`.
5. Crear al menos una categoría y un administrador en PostgreSQL. La contraseña del admin debe guardarse como hash BCrypt.
6. Ejecutar `dotnet run`.

## Incluye
- Inicio premium naval
- Catálogo y detalle de productos
- Carrito
- Checkout con transferencia
- Redirección a WhatsApp con detalle del pedido
- Guardado de pedidos en PostgreSQL
- Login admin con BCrypt
- Alta de productos con imagen
- Listado de pedidos

## Próximas mejoras sugeridas
- CRUD completo de categorías
- Editar/eliminar productos
- Varias imágenes por producto
- Gestión de estados de pedido
- Carga de comprobante
- Control de stock al confirmar
- Validaciones y antiforgery en todos los POST
- Seed inicial de administrador/categorías
