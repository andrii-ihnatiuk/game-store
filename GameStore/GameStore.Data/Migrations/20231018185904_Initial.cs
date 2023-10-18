using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Genres",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ParentGenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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

        migrationBuilder.CreateTable(
            name: "Platform",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Type = table.Column<string>(type: "nvarchar(450)", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Platform", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Publisher",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CompanyName = table.Column<string>(type: "nvarchar(40)", nullable: false),
                Description = table.Column<string>(type: "ntext", nullable: false),
                HomePage = table.Column<string>(type: "ntext", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Publisher", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Games",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Alias = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Price = table.Column<decimal>(type: "money", nullable: false),
                UnitInStock = table.Column<short>(type: "smallint", nullable: false),
                Discontinued = table.Column<bool>(type: "bit", nullable: false),
                PublisherId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Games", x => x.Id);
                table.ForeignKey(
                    name: "FK_Games_Publisher_PublisherId",
                    column: x => x.PublisherId,
                    principalTable: "Publisher",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "GameGenre",
            columns: table => new
            {
                GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                GenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GameGenre", x => new { x.GameId, x.GenreId });
                table.ForeignKey(
                    name: "FK_GameGenre_Games_GameId",
                    column: x => x.GameId,
                    principalTable: "Games",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_GameGenre_Genres_GenreId",
                    column: x => x.GenreId,
                    principalTable: "Genres",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "GamePlatform",
            columns: table => new
            {
                GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PlatformId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GamePlatform", x => new { x.GameId, x.PlatformId });
                table.ForeignKey(
                    name: "FK_GamePlatform_Games_GameId",
                    column: x => x.GameId,
                    principalTable: "Games",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_GamePlatform_Platform_PlatformId",
                    column: x => x.PlatformId,
                    principalTable: "Platform",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "Games",
            columns: new[] { "Id", "Alias", "Description", "Discontinued", "Name", "Price", "PublisherId", "UnitInStock" },
            values: new object[,]
            {
                { new Guid("737796ac-4643-4e90-a64a-67919aaf1fb2"), "gta-v", "An open-world action-adventure game.", true, "Grand Theft Auto V", 500m, null, (short)32 },
                { new Guid("9f7d2988-4a28-49d7-a154-7af4a2aedb0c"), "zelda-breath-of-the-wild", "An action-adventure game in an open world.", false, "The Legend of Zelda: Breath of the Wild", 1500.2m, null, (short)50 },
            });

        migrationBuilder.InsertData(
            table: "Genres",
            columns: new[] { "Id", "Name", "ParentGenreId" },
            values: new object[,]
            {
                { new Guid("3a021c30-43af-4a9c-8ab1-c73d5aad0cdd"), "Sports", null },
                { new Guid("5ed24324-d99b-4204-8483-3a2faaa528be"), "RPG", null },
                { new Guid("86147e44-96d5-4656-aa27-687afddecec2"), "Action", null },
                { new Guid("9503b4b7-a23e-4b98-a1dc-3e36bd48fc34"), "Puzzle & Skill", null },
                { new Guid("9f7d2988-4a28-49d7-a154-7af4a2aedb0c"), "Strategy", null },
                { new Guid("a6d7c660-2175-4800-ab08-6ef490e134d5"), "Adventure", null },
                { new Guid("d53f75f0-6206-4525-a288-b974c7b10c85"), "Races", null },
                { new Guid("d6c944ab-337c-49da-a714-d26db1e0580d"), "Misc.", null },
            });

        migrationBuilder.InsertData(
            table: "Platform",
            columns: new[] { "Id", "Type" },
            values: new object[,]
            {
                { new Guid("5ed24324-d99b-4204-8483-3a2faaa528be"), "console" },
                { new Guid("737796ac-4643-4e90-a64a-67919aaf1fb2"), "browser" },
                { new Guid("7379004c-7b90-4435-ac74-e9e5dae082fd"), "desktop" },
                { new Guid("9f7d2988-4a28-49d7-a154-7af4a2aedb0c"), "mobile" },
            });

        migrationBuilder.InsertData(
            table: "GameGenre",
            columns: new[] { "GameId", "GenreId" },
            values: new object[,]
            {
                { new Guid("737796ac-4643-4e90-a64a-67919aaf1fb2"), new Guid("86147e44-96d5-4656-aa27-687afddecec2") },
                { new Guid("9f7d2988-4a28-49d7-a154-7af4a2aedb0c"), new Guid("86147e44-96d5-4656-aa27-687afddecec2") },
                { new Guid("9f7d2988-4a28-49d7-a154-7af4a2aedb0c"), new Guid("a6d7c660-2175-4800-ab08-6ef490e134d5") },
            });

        migrationBuilder.InsertData(
            table: "GamePlatform",
            columns: new[] { "GameId", "PlatformId" },
            values: new object[,]
            {
                { new Guid("737796ac-4643-4e90-a64a-67919aaf1fb2"), new Guid("5ed24324-d99b-4204-8483-3a2faaa528be") },
                { new Guid("737796ac-4643-4e90-a64a-67919aaf1fb2"), new Guid("7379004c-7b90-4435-ac74-e9e5dae082fd") },
                { new Guid("9f7d2988-4a28-49d7-a154-7af4a2aedb0c"), new Guid("5ed24324-d99b-4204-8483-3a2faaa528be") },
            });

        migrationBuilder.InsertData(
            table: "Genres",
            columns: new[] { "Id", "Name", "ParentGenreId" },
            values: new object[,]
            {
                { new Guid("126cdfea-004a-4960-bcab-055dc845fd69"), "Formula", new Guid("d53f75f0-6206-4525-a288-b974c7b10c85") },
                { new Guid("1cf3e88f-834b-4072-84e5-d3d5a563daec"), "Rally", new Guid("d53f75f0-6206-4525-a288-b974c7b10c85") },
                { new Guid("3ecc8319-cf92-4c63-b8fc-4ed48aebdc5f"), "FPS", new Guid("86147e44-96d5-4656-aa27-687afddecec2") },
                { new Guid("737796ac-4643-4e90-a64a-67919aaf1fb2"), "RTS", new Guid("9f7d2988-4a28-49d7-a154-7af4a2aedb0c") },
                { new Guid("7379004c-7b90-4435-ac74-e9e5dae082fd"), "TBS", new Guid("9f7d2988-4a28-49d7-a154-7af4a2aedb0c") },
                { new Guid("b4dcf747-9299-4b84-9865-51727a91e059"), "TPS", new Guid("86147e44-96d5-4656-aa27-687afddecec2") },
                { new Guid("ea96b49c-79ea-4112-a508-bb9f908ad34b"), "Off-road", new Guid("d53f75f0-6206-4525-a288-b974c7b10c85") },
                { new Guid("fed36410-d4c2-44da-bbb4-d3f7e93b8d76"), "Arcade", new Guid("d53f75f0-6206-4525-a288-b974c7b10c85") },
            });

        migrationBuilder.CreateIndex(
            name: "IX_GameGenre_GenreId",
            table: "GameGenre",
            column: "GenreId");

        migrationBuilder.CreateIndex(
            name: "IX_GamePlatform_PlatformId",
            table: "GamePlatform",
            column: "PlatformId");

        migrationBuilder.CreateIndex(
            name: "IX_Games_Alias",
            table: "Games",
            column: "Alias",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Games_PublisherId",
            table: "Games",
            column: "PublisherId");

        migrationBuilder.CreateIndex(
            name: "IX_Genres_Name",
            table: "Genres",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Genres_ParentGenreId",
            table: "Genres",
            column: "ParentGenreId");

        migrationBuilder.CreateIndex(
            name: "IX_Platform_Type",
            table: "Platform",
            column: "Type",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "GameGenre");

        migrationBuilder.DropTable(
            name: "GamePlatform");

        migrationBuilder.DropTable(
            name: "Genres");

        migrationBuilder.DropTable(
            name: "Games");

        migrationBuilder.DropTable(
            name: "Platform");

        migrationBuilder.DropTable(
            name: "Publisher");
    }
}
