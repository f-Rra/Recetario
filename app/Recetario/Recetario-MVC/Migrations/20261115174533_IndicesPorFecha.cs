using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecetarioMVC.Migrations
{
    /// <inheritdoc />
    public partial class IndicesPorFecha : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MovimientosStock_Fecha",
                table: "MovimientosStock",
                column: "Fecha");

            migrationBuilder.CreateIndex(
                name: "IX_Comandas_Fecha",
                table: "Comandas",
                column: "Fecha");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MovimientosStock_Fecha",
                table: "MovimientosStock");

            migrationBuilder.DropIndex(
                name: "IX_Comandas_Fecha",
                table: "Comandas");
        }
    }
}
