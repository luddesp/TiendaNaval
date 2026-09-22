using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiendaNaval.Migrations
{
    /// <inheritdoc />
    public partial class AgregarDatosEnvio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Barrio",
                table: "Pedidos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Departamento",
                table: "Pedidos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Piso",
                table: "Pedidos",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Barrio",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Departamento",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Piso",
                table: "Pedidos");
        }
    }
}
