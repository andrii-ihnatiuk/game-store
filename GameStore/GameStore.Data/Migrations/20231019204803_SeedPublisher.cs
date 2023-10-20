using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class SeedPublisher : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "GameGenre",
            keyColumns: new[] { "GameId", "GenreId" },
            keyValues: new object[] { new Guid("737796ac-4643-4e90-a64a-67919aaf1fb2"), new Guid("86147e44-96d5-4656-aa27-687afddecec2") });

        migrationBuilder.DeleteData(
            table: "GameGenre",
            keyColumns: new[] { "GameId", "GenreId" },
            keyValues: new object[] { new Guid("9f7d2988-4a28-49d7-a154-7af4a2aedb0c"), new Guid("86147e44-96d5-4656-aa27-687afddecec2") });

        migrationBuilder.DeleteData(
            table: "GameGenre",
            keyColumns: new[] { "GameId", "GenreId" },
            keyValues: new object[] { new Guid("9f7d2988-4a28-49d7-a154-7af4a2aedb0c"), new Guid("a6d7c660-2175-4800-ab08-6ef490e134d5") });

        migrationBuilder.DeleteData(
            table: "GamePlatform",
            keyColumns: new[] { "GameId", "PlatformId" },
            keyValues: new object[] { new Guid("737796ac-4643-4e90-a64a-67919aaf1fb2"), new Guid("5ed24324-d99b-4204-8483-3a2faaa528be") });

        migrationBuilder.DeleteData(
            table: "GamePlatform",
            keyColumns: new[] { "GameId", "PlatformId" },
            keyValues: new object[] { new Guid("737796ac-4643-4e90-a64a-67919aaf1fb2"), new Guid("7379004c-7b90-4435-ac74-e9e5dae082fd") });

        migrationBuilder.DeleteData(
            table: "GamePlatform",
            keyColumns: new[] { "GameId", "PlatformId" },
            keyValues: new object[] { new Guid("9f7d2988-4a28-49d7-a154-7af4a2aedb0c"), new Guid("5ed24324-d99b-4204-8483-3a2faaa528be") });

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("126cdfea-004a-4960-bcab-055dc845fd69"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("1cf3e88f-834b-4072-84e5-d3d5a563daec"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("3a021c30-43af-4a9c-8ab1-c73d5aad0cdd"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("3ecc8319-cf92-4c63-b8fc-4ed48aebdc5f"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("5ed24324-d99b-4204-8483-3a2faaa528be"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("737796ac-4643-4e90-a64a-67919aaf1fb2"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("7379004c-7b90-4435-ac74-e9e5dae082fd"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("9503b4b7-a23e-4b98-a1dc-3e36bd48fc34"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("b4dcf747-9299-4b84-9865-51727a91e059"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("d6c944ab-337c-49da-a714-d26db1e0580d"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("ea96b49c-79ea-4112-a508-bb9f908ad34b"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("fed36410-d4c2-44da-bbb4-d3f7e93b8d76"));

        migrationBuilder.DeleteData(
            table: "Platform",
            keyColumn: "Id",
            keyValue: new Guid("737796ac-4643-4e90-a64a-67919aaf1fb2"));

        migrationBuilder.DeleteData(
            table: "Platform",
            keyColumn: "Id",
            keyValue: new Guid("9f7d2988-4a28-49d7-a154-7af4a2aedb0c"));

        migrationBuilder.DeleteData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("737796ac-4643-4e90-a64a-67919aaf1fb2"));

        migrationBuilder.DeleteData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("9f7d2988-4a28-49d7-a154-7af4a2aedb0c"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("86147e44-96d5-4656-aa27-687afddecec2"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("9f7d2988-4a28-49d7-a154-7af4a2aedb0c"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("a6d7c660-2175-4800-ab08-6ef490e134d5"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("d53f75f0-6206-4525-a288-b974c7b10c85"));

        migrationBuilder.DeleteData(
            table: "Platform",
            keyColumn: "Id",
            keyValue: new Guid("5ed24324-d99b-4204-8483-3a2faaa528be"));

        migrationBuilder.DeleteData(
            table: "Platform",
            keyColumn: "Id",
            keyValue: new Guid("7379004c-7b90-4435-ac74-e9e5dae082fd"));

        migrationBuilder.InsertData(
            table: "Genres",
            columns: new[] { "Id", "Name", "ParentGenreId" },
            values: new object[,]
            {
                { new Guid("073f790e-a105-491d-965c-946e841c3b3e"), "Strategy", null },
                { new Guid("13ba20d2-42fc-4eaa-a7f9-900782cabfc3"), "Sports", null },
                { new Guid("5efe1909-c0b0-440e-8915-2831570816ec"), "RPG", null },
                { new Guid("956c4baf-e989-4046-b6cb-1d98df33cf6d"), "Misc.", null },
                { new Guid("b3562f18-5e7c-411a-ab76-b675b78bd23d"), "Races", null },
                { new Guid("d9eaef23-9680-4d25-869b-4ed91847fd03"), "Puzzle & Skill", null },
                { new Guid("dd2bf352-9cfd-4b88-b46f-08217104f90d"), "Adventure", null },
                { new Guid("e96511b9-e864-44cc-899f-8313609ffb63"), "Action", null },
            });

        migrationBuilder.InsertData(
            table: "Platform",
            columns: new[] { "Id", "Type" },
            values: new object[,]
            {
                { new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"), "browser" },
                { new Guid("073f790e-a105-491d-965c-946e841c3b3e"), "mobile" },
                { new Guid("5efe1909-c0b0-440e-8915-2831570816ec"), "console" },
                { new Guid("ead8a2bf-e751-47a9-a2fc-e8b6f643b0c8"), "desktop" },
            });

        migrationBuilder.InsertData(
            table: "Publisher",
            columns: new[] { "Id", "CompanyName", "Description", "HomePage" },
            values: new object[,]
            {
                { new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"), "Electronic Arts", "Electronic Arts Inc. is a global leader in digital interactive entertainment.", "https://www.ea.com/" },
                { new Guid("073f790e-a105-491d-965c-946e841c3b3e"), "Activision", "Activision Publishing, Inc. is an American video game publisher based in Santa Monica, California.", "https://www.activision.com/" },
                { new Guid("ead8a2bf-e751-47a9-a2fc-e8b6f643b0c8"), "Ubisoft", "Ubisoft Entertainment SA is a French video game company headquartered in Montreal.", "https://www.ubisoft.com/" },
            });

        migrationBuilder.InsertData(
            table: "Games",
            columns: new[] { "Id", "Alias", "Description", "Discontinued", "Name", "Price", "PublisherId", "UnitInStock" },
            values: new object[,]
            {
                { new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"), "gta-v", "An open-world action-adventure game.", true, "Grand Theft Auto V", 500m, new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"), (short)32 },
                { new Guid("073f790e-a105-491d-965c-946e841c3b3e"), "zelda-breath-of-the-wild", "An action-adventure game in an open world.", false, "The Legend of Zelda: Breath of the Wild", 1500.2m, new Guid("073f790e-a105-491d-965c-946e841c3b3e"), (short)50 },
            });

        migrationBuilder.InsertData(
            table: "Genres",
            columns: new[] { "Id", "Name", "ParentGenreId" },
            values: new object[,]
            {
                { new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"), "RTS", new Guid("073f790e-a105-491d-965c-946e841c3b3e") },
                { new Guid("2ba0604c-5515-470c-a500-58b60e8be000"), "Rally", new Guid("b3562f18-5e7c-411a-ab76-b675b78bd23d") },
                { new Guid("5d7545f5-a77a-4632-89c6-40f8756d2f75"), "Off-road", new Guid("b3562f18-5e7c-411a-ab76-b675b78bd23d") },
                { new Guid("837e2c69-c602-4038-9754-47b9c005a0a8"), "TPS", new Guid("e96511b9-e864-44cc-899f-8313609ffb63") },
                { new Guid("927cc631-c4f1-4feb-8a0c-9db6cd43402a"), "Formula", new Guid("b3562f18-5e7c-411a-ab76-b675b78bd23d") },
                { new Guid("e0e725ed-50ed-4ff7-94e7-38d0e1d2fa39"), "Arcade", new Guid("b3562f18-5e7c-411a-ab76-b675b78bd23d") },
                { new Guid("ead8a2bf-e751-47a9-a2fc-e8b6f643b0c8"), "TBS", new Guid("073f790e-a105-491d-965c-946e841c3b3e") },
                { new Guid("f82e27c8-760e-4cb8-866f-60bb33caeaac"), "FPS", new Guid("e96511b9-e864-44cc-899f-8313609ffb63") },
            });

        migrationBuilder.InsertData(
            table: "GameGenre",
            columns: new[] { "GameId", "GenreId" },
            values: new object[,]
            {
                { new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"), new Guid("e96511b9-e864-44cc-899f-8313609ffb63") },
                { new Guid("073f790e-a105-491d-965c-946e841c3b3e"), new Guid("dd2bf352-9cfd-4b88-b46f-08217104f90d") },
                { new Guid("073f790e-a105-491d-965c-946e841c3b3e"), new Guid("e96511b9-e864-44cc-899f-8313609ffb63") },
            });

        migrationBuilder.InsertData(
            table: "GamePlatform",
            columns: new[] { "GameId", "PlatformId" },
            values: new object[,]
            {
                { new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"), new Guid("5efe1909-c0b0-440e-8915-2831570816ec") },
                { new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"), new Guid("ead8a2bf-e751-47a9-a2fc-e8b6f643b0c8") },
                { new Guid("073f790e-a105-491d-965c-946e841c3b3e"), new Guid("5efe1909-c0b0-440e-8915-2831570816ec") },
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "GameGenre",
            keyColumns: new[] { "GameId", "GenreId" },
            keyValues: new object[] { new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"), new Guid("e96511b9-e864-44cc-899f-8313609ffb63") });

        migrationBuilder.DeleteData(
            table: "GameGenre",
            keyColumns: new[] { "GameId", "GenreId" },
            keyValues: new object[] { new Guid("073f790e-a105-491d-965c-946e841c3b3e"), new Guid("dd2bf352-9cfd-4b88-b46f-08217104f90d") });

        migrationBuilder.DeleteData(
            table: "GameGenre",
            keyColumns: new[] { "GameId", "GenreId" },
            keyValues: new object[] { new Guid("073f790e-a105-491d-965c-946e841c3b3e"), new Guid("e96511b9-e864-44cc-899f-8313609ffb63") });

        migrationBuilder.DeleteData(
            table: "GamePlatform",
            keyColumns: new[] { "GameId", "PlatformId" },
            keyValues: new object[] { new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"), new Guid("5efe1909-c0b0-440e-8915-2831570816ec") });

        migrationBuilder.DeleteData(
            table: "GamePlatform",
            keyColumns: new[] { "GameId", "PlatformId" },
            keyValues: new object[] { new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"), new Guid("ead8a2bf-e751-47a9-a2fc-e8b6f643b0c8") });

        migrationBuilder.DeleteData(
            table: "GamePlatform",
            keyColumns: new[] { "GameId", "PlatformId" },
            keyValues: new object[] { new Guid("073f790e-a105-491d-965c-946e841c3b3e"), new Guid("5efe1909-c0b0-440e-8915-2831570816ec") });

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("13ba20d2-42fc-4eaa-a7f9-900782cabfc3"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("2ba0604c-5515-470c-a500-58b60e8be000"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("5d7545f5-a77a-4632-89c6-40f8756d2f75"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("5efe1909-c0b0-440e-8915-2831570816ec"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("837e2c69-c602-4038-9754-47b9c005a0a8"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("927cc631-c4f1-4feb-8a0c-9db6cd43402a"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("956c4baf-e989-4046-b6cb-1d98df33cf6d"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("d9eaef23-9680-4d25-869b-4ed91847fd03"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("e0e725ed-50ed-4ff7-94e7-38d0e1d2fa39"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("ead8a2bf-e751-47a9-a2fc-e8b6f643b0c8"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("f82e27c8-760e-4cb8-866f-60bb33caeaac"));

        migrationBuilder.DeleteData(
            table: "Platform",
            keyColumn: "Id",
            keyValue: new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"));

        migrationBuilder.DeleteData(
            table: "Platform",
            keyColumn: "Id",
            keyValue: new Guid("073f790e-a105-491d-965c-946e841c3b3e"));

        migrationBuilder.DeleteData(
            table: "Publisher",
            keyColumn: "Id",
            keyValue: new Guid("ead8a2bf-e751-47a9-a2fc-e8b6f643b0c8"));

        migrationBuilder.DeleteData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"));

        migrationBuilder.DeleteData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("073f790e-a105-491d-965c-946e841c3b3e"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("073f790e-a105-491d-965c-946e841c3b3e"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("b3562f18-5e7c-411a-ab76-b675b78bd23d"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("dd2bf352-9cfd-4b88-b46f-08217104f90d"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("e96511b9-e864-44cc-899f-8313609ffb63"));

        migrationBuilder.DeleteData(
            table: "Platform",
            keyColumn: "Id",
            keyValue: new Guid("5efe1909-c0b0-440e-8915-2831570816ec"));

        migrationBuilder.DeleteData(
            table: "Platform",
            keyColumn: "Id",
            keyValue: new Guid("ead8a2bf-e751-47a9-a2fc-e8b6f643b0c8"));

        migrationBuilder.DeleteData(
            table: "Publisher",
            keyColumn: "Id",
            keyValue: new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"));

        migrationBuilder.DeleteData(
            table: "Publisher",
            keyColumn: "Id",
            keyValue: new Guid("073f790e-a105-491d-965c-946e841c3b3e"));

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
    }
}
