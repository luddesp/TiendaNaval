using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiendaNaval.Migrations
{
    /// <inheritdoc />
    public partial class AgregarOpcionesProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UsaEspecialidad",
                table: "Productos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UsaJerarquia",
                table: "Productos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UsaTipoTrasero",
                table: "Productos",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsaEspecialidad",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "UsaJerarquia",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "UsaTipoTrasero",
                table: "Productos");
        }
    }
}
