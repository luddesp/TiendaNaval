using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiendaNaval.Migrations
{
    /// <inheritdoc />
    public partial class AgregarMetodoEnvio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MetodoEnvio",
                table: "Pedidos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetodoEnvio",
                table: "Pedidos");
        }
    }
}
