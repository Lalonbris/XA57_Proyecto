using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XA57_Proyecto.Migrations
{
    /// <inheritdoc />
    public partial class AddColorAndColorHexToPedidos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "color",
                table: "Pedidos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "color_hex",
                table: "Pedidos",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "color",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "color_hex",
                table: "Pedidos");
        }
    }
}
