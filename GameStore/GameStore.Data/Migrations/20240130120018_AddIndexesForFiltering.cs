using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class AddIndexesForFiltering : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Game",
            type: "nvarchar(450)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.CreateIndex(
            name: "IX_Game_Name",
            table: "Game",
            column: "Name");

        migrationBuilder.CreateIndex(
            name: "IX_Game_PublishDate",
            table: "Game",
            column: "PublishDate");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Game_Name",
            table: "Game");

        migrationBuilder.DropIndex(
            name: "IX_Game_PublishDate",
            table: "Game");

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Game",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)");
    }
}
