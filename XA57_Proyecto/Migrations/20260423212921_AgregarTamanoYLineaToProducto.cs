using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XA57_Proyecto.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTamanoYLineaToProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "linea_id",
                table: "productos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tamano",
                table: "productos",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_productos_linea_id",
                table: "productos",
                column: "linea_id");

            migrationBuilder.AddForeignKey(
                name: "FK_productos_lineas_linea_id",
                table: "productos",
                column: "linea_id",
                principalTable: "lineas",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productos_lineas_linea_id",
                table: "productos");

            migrationBuilder.DropIndex(
                name: "IX_productos_linea_id",
                table: "productos");

            migrationBuilder.DropColumn(
                name: "linea_id",
                table: "productos");

            migrationBuilder.DropColumn(
                name: "tamano",
                table: "productos");
        }
    }
}
