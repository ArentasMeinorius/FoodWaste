using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace FoodWaste.Migrations
{
    public partial class MinorDatabaseFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTemporaryProducts",
                table: "UserTemporaryProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSelectedAllergens",
                table: "UserSelectedAllergens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCreatedAllergens",
                table: "UserCreatedAllergens");

            migrationBuilder.RenameTable(
                name: "UserTemporaryProducts",
                newName: "TemporaryProducts");

            migrationBuilder.RenameTable(
                name: "UserSelectedAllergens",
                newName: "SelectedAllergens");

            migrationBuilder.RenameTable(
                name: "UserCreatedAllergens",
                newName: "CreatedAllergens");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "TemporaryProducts",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SelectedAllergens",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CreatedAllergens",
                newName: "UserId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "SelectedAllergens",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "CreatedAllergens",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TemporaryProducts",
                table: "TemporaryProducts",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SelectedAllergens",
                table: "SelectedAllergens",
                columns: new[] { "AllergenId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreatedAllergens",
                table: "CreatedAllergens",
                columns: new[] { "AllergenId", "UserId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TemporaryProducts",
                table: "TemporaryProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SelectedAllergens",
                table: "SelectedAllergens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreatedAllergens",
                table: "CreatedAllergens");

            migrationBuilder.RenameTable(
                name: "TemporaryProducts",
                newName: "UserTemporaryProducts");

            migrationBuilder.RenameTable(
                name: "SelectedAllergens",
                newName: "UserSelectedAllergens");

            migrationBuilder.RenameTable(
                name: "CreatedAllergens",
                newName: "UserCreatedAllergens");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserTemporaryProducts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserSelectedAllergens",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserCreatedAllergens",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UserSelectedAllergens",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UserCreatedAllergens",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTemporaryProducts",
                table: "UserTemporaryProducts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSelectedAllergens",
                table: "UserSelectedAllergens",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCreatedAllergens",
                table: "UserCreatedAllergens",
                column: "Id");
        }
    }
}
