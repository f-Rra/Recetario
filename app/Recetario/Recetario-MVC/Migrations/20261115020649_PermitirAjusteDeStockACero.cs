using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecetarioMVC.Migrations
{
    /// <inheritdoc />
    public partial class PermitirAjusteDeStockACero : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_MovimientosStock_Cantidad",
                table: "MovimientosStock");

            migrationBuilder.AddCheckConstraint(
                name: "CK_MovimientosStock_Cantidad",
                table: "MovimientosStock",
                sql: "Cantidad > 0 OR (Tipo = 3 AND Cantidad = 0)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_MovimientosStock_Cantidad",
                table: "MovimientosStock");

            migrationBuilder.AddCheckConstraint(
                name: "CK_MovimientosStock_Cantidad",
                table: "MovimientosStock",
                sql: "Cantidad > 0");
        }
    }
}
