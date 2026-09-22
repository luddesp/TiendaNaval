using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiendaNaval.Migrations
{
    /// <inheritdoc />
    public partial class AgregarJerarquiaAspirantes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JerarquiaAspirantes",
                table: "Productos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TieneJerarquiaAspirantes",
                table: "Productos",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JerarquiaAspirantes",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "TieneJerarquiaAspirantes",
                table: "Productos");
        }
    }
}
