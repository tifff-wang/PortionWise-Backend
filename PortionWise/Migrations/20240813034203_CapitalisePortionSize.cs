using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortionWise.Migrations
{
    /// <inheritdoc />
    public partial class CapitalisePortionSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "portionSize",
                table: "Recipes",
                newName: "PortionSize");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PortionSize",
                table: "Recipes",
                newName: "portionSize");
        }
    }
}
