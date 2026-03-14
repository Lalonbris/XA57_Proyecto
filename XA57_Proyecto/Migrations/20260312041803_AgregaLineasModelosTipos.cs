using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace XA57_Proyecto.Migrations
{
    /// <inheritdoc />
    public partial class AgregaLineasModelosTipos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "categoria",
                table: "productos");

            migrationBuilder.DropColumn(
                name: "color",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "numero_serie",
                table: "Pedidos");

            migrationBuilder.AlterColumn<string>(
                name: "imagen_url",
                table: "productos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "descripcion",
                table: "productos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<bool>(
                name: "activo",
                table: "productos",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "tipo_producto_id",
                table: "productos",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "notas_especiales",
                table: "Pedidos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "linea_id",
                table: "Pedidos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "modelo_autobus_id",
                table: "Pedidos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nombre_operador",
                table: "Pedidos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "numero_economico",
                table: "Pedidos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ruta",
                table: "Pedidos",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "lineas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    color_primario = table.Column<string>(type: "text", nullable: true),
                    color_secundario = table.Column<string>(type: "text", nullable: true),
                    nombre_operador = table.Column<string>(type: "text", nullable: true),
                    logo_url = table.Column<string>(type: "text", nullable: true),
                    activa = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lineas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "modelos_autobus",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    fabricante = table.Column<string>(type: "text", nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_modelos_autobus", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tipos_producto",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    max_caracteres = table.Column<int>(type: "integer", nullable: false),
                    permite_nombre = table.Column<bool>(type: "boolean", nullable: false),
                    permite_numero_economico = table.Column<bool>(type: "boolean", nullable: false),
                    permite_ruta = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipos_producto", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_productos_tipo_producto_id",
                table: "productos",
                column: "tipo_producto_id");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_linea_id",
                table: "Pedidos",
                column: "linea_id");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_modelo_autobus_id",
                table: "Pedidos",
                column: "modelo_autobus_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_lineas_linea_id",
                table: "Pedidos",
                column: "linea_id",
                principalTable: "lineas",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_modelos_autobus_modelo_autobus_id",
                table: "Pedidos",
                column: "modelo_autobus_id",
                principalTable: "modelos_autobus",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_productos_tipos_producto_tipo_producto_id",
                table: "productos",
                column: "tipo_producto_id",
                principalTable: "tipos_producto",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_lineas_linea_id",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_modelos_autobus_modelo_autobus_id",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_productos_tipos_producto_tipo_producto_id",
                table: "productos");

            migrationBuilder.DropTable(
                name: "lineas");

            migrationBuilder.DropTable(
                name: "modelos_autobus");

            migrationBuilder.DropTable(
                name: "tipos_producto");

            migrationBuilder.DropIndex(
                name: "IX_productos_tipo_producto_id",
                table: "productos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_linea_id",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_modelo_autobus_id",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "activo",
                table: "productos");

            migrationBuilder.DropColumn(
                name: "tipo_producto_id",
                table: "productos");

            migrationBuilder.DropColumn(
                name: "linea_id",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "modelo_autobus_id",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "nombre_operador",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "numero_economico",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "ruta",
                table: "Pedidos");

            migrationBuilder.AlterColumn<string>(
                name: "imagen_url",
                table: "productos",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "descripcion",
                table: "productos",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "categoria",
                table: "productos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "notas_especiales",
                table: "Pedidos",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "color",
                table: "Pedidos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "numero_serie",
                table: "Pedidos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
