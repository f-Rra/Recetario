using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecetarioMVC.Migrations
{
    /// <inheritdoc />
    public partial class GuardarPasosDeLaComanda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PasosComanda",
                columns: table => new
                {
                    IdPasoComanda = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdComanda = table.Column<int>(type: "int", nullable: false),
                    NroPaso = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasosComanda", x => x.IdPasoComanda);
                    table.ForeignKey(
                        name: "FK_PasosComanda_Comandas_IdComanda",
                        column: x => x.IdComanda,
                        principalTable: "Comandas",
                        principalColumn: "IdComanda",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PasosComanda_IdComanda_NroPaso",
                table: "PasosComanda",
                columns: new[] { "IdComanda", "NroPaso" },
                unique: true);

            // Las comandas que ya existen: no hay registro de cómo era el procedimiento el día
            // que se cocinaron, así que se les copia el que la receta tiene hoy. Es lo mismo
            // que venían mostrando; a partir de acá ya no cambia.
            migrationBuilder.Sql(@"
                INSERT INTO PasosComanda (IdComanda, NroPaso, Descripcion)
                SELECT c.IdComanda, p.NroPaso, p.Descripcion
                FROM Comandas c
                INNER JOIN Procedimientos p ON p.IdReceta = c.IdReceta;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PasosComanda");
        }
    }
}
