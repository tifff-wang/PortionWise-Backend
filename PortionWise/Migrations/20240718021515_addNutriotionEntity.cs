using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortionWise.Migrations
{
    /// <inheritdoc />
    public partial class addNutriotionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NutritionInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SugarGram = table.Column<double>(type: "REAL", nullable: false),
                    FiberGram = table.Column<double>(type: "REAL", nullable: false),
                    ServingSize = table.Column<double>(type: "REAL", nullable: false),
                    SodiumMg = table.Column<double>(type: "REAL", nullable: false),
                    PotassiumMg = table.Column<double>(type: "REAL", nullable: false),
                    FatSaturatedGram = table.Column<double>(type: "REAL", nullable: false),
                    FatTotalGram = table.Column<double>(type: "REAL", nullable: false),
                    Calories = table.Column<double>(type: "REAL", nullable: false),
                    CholesterolMg = table.Column<double>(type: "REAL", nullable: false),
                    ProteinGram = table.Column<double>(type: "REAL", nullable: false),
                    CarbohydratesTotalGram = table.Column<double>(type: "REAL", nullable: false),
                    RecipeId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutritionInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NutritionInfo_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NutritionInfo_RecipeId",
                table: "NutritionInfo",
                column: "RecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NutritionInfo");
        }
    }
}
