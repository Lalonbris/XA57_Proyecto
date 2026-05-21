using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XA57_Proyecto.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTiemposProduccion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "tiempo_produccion_fin",
                table: "Pedidos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "tiempo_produccion_inicio",
                table: "Pedidos",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tiempo_produccion_fin",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "tiempo_produccion_inicio",
                table: "Pedidos");
        }
    }
}
