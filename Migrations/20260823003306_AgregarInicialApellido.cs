using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiendaNaval.Migrations
{
    /// <inheritdoc />
    public partial class AgregarInicialApellido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UsaApellido",
                table: "Productos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UsaInicial",
                table: "Productos",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsaApellido",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "UsaInicial",
                table: "Productos");
        }
    }
}
