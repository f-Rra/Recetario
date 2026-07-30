using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecetarioMVC.Migrations
{
    /// <inheritdoc />
    public partial class AgregarDepositoAIngrediente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Deposito",
                table: "Ingredientes",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deposito",
                table: "Ingredientes");
        }
    }
}
