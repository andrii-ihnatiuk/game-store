using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class UpdatedGameEntity : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_Games",
            table: "Games");

        migrationBuilder.AddColumn<long>(
            name: "Id",
            table: "Games",
            type: "bigint",
            nullable: false,
            defaultValue: 0L)
            .Annotation("SqlServer:Identity", "1, 1");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Games",
            table: "Games",
            column: "Id");

        migrationBuilder.CreateIndex(
            name: "IX_Games_Alias",
            table: "Games",
            column: "Alias",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_Games",
            table: "Games");

        migrationBuilder.DropIndex(
            name: "IX_Games_Alias",
            table: "Games");

        migrationBuilder.DropColumn(
            name: "Id",
            table: "Games");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Games",
            table: "Games",
            column: "Alias");
    }
}
