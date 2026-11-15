using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecetarioMVC.Migrations
{
    /// <inheritdoc />
    public partial class GuardarIngredientesDeLaComanda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IngredientesComanda",
                columns: table => new
                {
                    IdIngredienteComanda = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdComanda = table.Column<int>(type: "int", nullable: false),
                    IdIngrediente = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(10,4)", precision: 10, scale: 4, nullable: false),
                    IdUnidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientesComanda", x => x.IdIngredienteComanda);
                    table.ForeignKey(
                        name: "FK_IngredientesComanda_Comandas_IdComanda",
                        column: x => x.IdComanda,
                        principalTable: "Comandas",
                        principalColumn: "IdComanda",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IngredientesComanda_Ingredientes_IdIngrediente",
                        column: x => x.IdIngrediente,
                        principalTable: "Ingredientes",
                        principalColumn: "IdIngrediente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IngredientesComanda_Unidades_IdUnidad",
                        column: x => x.IdUnidad,
                        principalTable: "Unidades",
                        principalColumn: "IdUnidad",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IngredientesComanda_IdComanda",
                table: "IngredientesComanda",
                column: "IdComanda");

            migrationBuilder.CreateIndex(
                name: "IX_IngredientesComanda_IdIngrediente",
                table: "IngredientesComanda",
                column: "IdIngrediente");

            migrationBuilder.CreateIndex(
                name: "IX_IngredientesComanda_IdUnidad",
                table: "IngredientesComanda",
                column: "IdUnidad");

            // Las comandas que ya existen: lo que se cocinó se reconstruye a partir de las
            // salidas de stock que generó cada una ("Consumo comanda #N", Tipo 2 = salida),
            // que ya tenían las modificaciones aplicadas. Sin esto, su detalle seguiría
            // saliendo de la receta de hoy.
            migrationBuilder.Sql(@"
                INSERT INTO IngredientesComanda (IdComanda, IdIngrediente, Cantidad, IdUnidad)
                SELECT c.IdComanda, m.IdIngrediente, m.Cantidad, m.IdUnidad
                FROM MovimientosStock m
                INNER JOIN Comandas c
                    ON m.Observaciones = N'Consumo comanda #' + CAST(c.IdComanda AS nvarchar(10))
                WHERE m.Tipo = 2
                ORDER BY m.IdMovimiento;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IngredientesComanda");
        }
    }
}
