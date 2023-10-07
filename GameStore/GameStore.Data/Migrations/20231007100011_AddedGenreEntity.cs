using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class AddedGenreEntity : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<long>(
            name: "GenreId",
            table: "Games",
            type: "bigint",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "Genres",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ParentGenreId = table.Column<long>(type: "bigint", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Genres", x => x.Id);
                table.ForeignKey(
                    name: "FK_Genres_Genres_ParentGenreId",
                    column: x => x.ParentGenreId,
                    principalTable: "Genres",
                    principalColumn: "Id");
            });

        migrationBuilder.InsertData(
            table: "Genres",
            columns: new[] { "Id", "Name", "ParentGenreId" },
            values: new object[,]
            {
                { 1L, "Strategy", null },
                { 4L, "RPG", null },
                { 5L, "Sports", null },
                { 6L, "Races", null },
                { 11L, "Action", null },
                { 14L, "Adventure", null },
                { 15L, "Puzzle & Skill", null },
                { 16L, "Misc.", null },
            });

        migrationBuilder.InsertData(
            table: "Games",
            columns: new[] { "Id", "Alias", "Description", "GenreId", "Name" },
            values: new object[,]
            {
                { 1L, "zelda-breath-of-the-wild", "An action-adventure game in an open world.", 14L, "The Legend of Zelda: Breath of the Wild" },
                { 2L, "gta-v", "An open-world action-adventure game.", 11L, "Grand Theft Auto V" },
            });

        migrationBuilder.InsertData(
            table: "Genres",
            columns: new[] { "Id", "Name", "ParentGenreId" },
            values: new object[,]
            {
                { 2L, "RTS", 1L },
                { 3L, "TBS", 1L },
                { 7L, "Rally", 6L },
                { 8L, "Arcade", 6L },
                { 9L, "Formula", 6L },
                { 10L, "Off-road", 6L },
                { 12L, "FPS", 11L },
                { 13L, "TPS", 11L },
            });

        migrationBuilder.CreateIndex(
            name: "IX_Games_GenreId",
            table: "Games",
            column: "GenreId");

        migrationBuilder.CreateIndex(
            name: "IX_Genres_Name",
            table: "Genres",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Genres_ParentGenreId",
            table: "Genres",
            column: "ParentGenreId");

        migrationBuilder.AddForeignKey(
            name: "FK_Games_Genres_GenreId",
            table: "Games",
            column: "GenreId",
            principalTable: "Genres",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Games_Genres_GenreId",
            table: "Games");

        migrationBuilder.DropTable(
            name: "Genres");

        migrationBuilder.DropIndex(
            name: "IX_Games_GenreId",
            table: "Games");

        migrationBuilder.DeleteData(
            table: "Games",
            keyColumn: "Id",
            keyValue: 1L);

        migrationBuilder.DeleteData(
            table: "Games",
            keyColumn: "Id",
            keyValue: 2L);

        migrationBuilder.DropColumn(
            name: "GenreId",
            table: "Games");
    }
}
