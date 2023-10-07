using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class AddedPlatformEntity : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<long>(
            name: "PlatformId",
            table: "Games",
            type: "bigint",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "Platform",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Type = table.Column<string>(type: "nvarchar(450)", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Platform", x => x.Id);
            });

        migrationBuilder.UpdateData(
            table: "Games",
            keyColumn: "Id",
            keyValue: 1L,
            column: "PlatformId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Games",
            keyColumn: "Id",
            keyValue: 2L,
            column: "PlatformId",
            value: null);

        migrationBuilder.InsertData(
            table: "Platform",
            columns: new[] { "Id", "Type" },
            values: new object[,]
            {
                { 1L, "mobile" },
                { 2L, "browser" },
                { 3L, "desktop" },
                { 4L, "console" },
            });

        migrationBuilder.CreateIndex(
            name: "IX_Games_PlatformId",
            table: "Games",
            column: "PlatformId");

        migrationBuilder.CreateIndex(
            name: "IX_Platform_Type",
            table: "Platform",
            column: "Type",
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_Games_Platform_PlatformId",
            table: "Games",
            column: "PlatformId",
            principalTable: "Platform",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Games_Platform_PlatformId",
            table: "Games");

        migrationBuilder.DropTable(
            name: "Platform");

        migrationBuilder.DropIndex(
            name: "IX_Games_PlatformId",
            table: "Games");

        migrationBuilder.DropColumn(
            name: "PlatformId",
            table: "Games");
    }
}
