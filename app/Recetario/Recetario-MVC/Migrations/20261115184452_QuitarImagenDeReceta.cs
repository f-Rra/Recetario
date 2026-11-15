using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecetarioMVC.Migrations
{
    /// <inheritdoc />
    public partial class QuitarImagenDeReceta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagen",
                table: "Recetas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Imagen",
                table: "Recetas",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}
