using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortionWise.Migrations
{
    /// <inheritdoc />
    public partial class addNutriotionInfoCacheExpirationDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CacheExpirationTime",
                table: "NutritionInfo",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CacheExpirationTime",
                table: "NutritionInfo");
        }
    }
}
