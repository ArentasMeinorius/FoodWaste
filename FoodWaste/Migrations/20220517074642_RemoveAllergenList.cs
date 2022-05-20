using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodWaste.Migrations
{
    public partial class RemoveAllergenList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Allergens_Product_ProductId",
                table: "Allergens");

            migrationBuilder.DropIndex(
                name: "IX_Allergens_ProductId",
                table: "Allergens");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Allergens");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Allergens",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Allergens_ProductId",
                table: "Allergens",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Allergens_Product_ProductId",
                table: "Allergens",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
