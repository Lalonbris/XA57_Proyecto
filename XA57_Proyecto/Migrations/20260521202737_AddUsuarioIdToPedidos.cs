using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XA57_Proyecto.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuarioIdToPedidos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "usuario_id",
                table: "Pedidos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_usuario_id",
                table: "Pedidos",
                column: "usuario_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_AspNetUsers_usuario_id",
                table: "Pedidos",
                column: "usuario_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_AspNetUsers_usuario_id",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_usuario_id",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "usuario_id",
                table: "Pedidos");
        }
    }
}
