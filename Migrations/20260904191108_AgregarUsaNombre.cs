using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiendaNaval.Migrations
{
    /// <inheritdoc />
    public partial class AgregarUsaNombre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UsaNombre",
                table: "Productos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Apellido",
                table: "DetallePedidos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "DetallePedidos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConGanchito",
                table: "DetallePedidos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConNombre",
                table: "DetallePedidos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Especialidad",
                table: "DetallePedidos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Inicial",
                table: "DetallePedidos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Jerarquia",
                table: "DetallePedidos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JerarquiaAspirantes",
                table: "DetallePedidos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreApellido",
                table: "DetallePedidos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombrePersonalizado",
                table: "DetallePedidos",
                type: "character varying(12)",
                maxLength: 12,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsaNombre",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Apellido",
                table: "DetallePedidos");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "DetallePedidos");

            migrationBuilder.DropColumn(
                name: "ConGanchito",
                table: "DetallePedidos");

            migrationBuilder.DropColumn(
                name: "ConNombre",
                table: "DetallePedidos");

            migrationBuilder.DropColumn(
                name: "Especialidad",
                table: "DetallePedidos");

            migrationBuilder.DropColumn(
                name: "Inicial",
                table: "DetallePedidos");

            migrationBuilder.DropColumn(
                name: "Jerarquia",
                table: "DetallePedidos");

            migrationBuilder.DropColumn(
                name: "JerarquiaAspirantes",
                table: "DetallePedidos");

            migrationBuilder.DropColumn(
                name: "NombreApellido",
                table: "DetallePedidos");

            migrationBuilder.DropColumn(
                name: "NombrePersonalizado",
                table: "DetallePedidos");
        }
    }
}
