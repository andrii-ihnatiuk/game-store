using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class AddTranslationForGenreAndPlatform : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "GenreTranslation",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GenreTranslation", x => x.Id);
                table.ForeignKey(
                    name: "FK_GenreTranslation_Genre_CoreId",
                    column: x => x.CoreId,
                    principalTable: "Genre",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "PlatformTranslation",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PlatformTranslation", x => x.Id);
                table.ForeignKey(
                    name: "FK_PlatformTranslation_Platform_CoreId",
                    column: x => x.CoreId,
                    principalTable: "Platform",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "GenreTranslation",
            columns: new[] { "Id", "CoreId", "LanguageCode", "Name" },
            values: new object[,]
            {
                { new Guid("110f9f7e-1e88-4b12-a090-693897fbdc4a"), new Guid("e0e725ed-50ed-4ff7-94e7-38d0e1d2fa39"), "uk-UA", "Аркада" },
                { new Guid("18b35271-7959-4782-a456-05c6450c1d01"), new Guid("5d7545f5-a77a-4632-89c6-40f8756d2f75"), "uk-UA", "Бездоріжжя" },
                { new Guid("1d69066b-62d2-40e3-93e5-ed33c7a93dce"), new Guid("2ba0604c-5515-470c-a500-58b60e8be000"), "uk-UA", "Ралі" },
                { new Guid("3dba4278-245a-47ac-9fbd-2d340c85f1d1"), new Guid("d9eaef23-9680-4d25-869b-4ed91847fd03"), "uk-UA", "Головоломки" },
                { new Guid("63d580f5-5ee9-4da6-9f4e-00e4607d48e3"), new Guid("073f790e-a105-491d-965c-946e841c3b3e"), "uk-UA", "Стратегія" },
                { new Guid("a3bed4b2-77db-4277-9089-50432871c8ad"), new Guid("13ba20d2-42fc-4eaa-a7f9-900782cabfc3"), "uk-UA", "Спорт" },
                { new Guid("b78bf3c6-8214-41bc-b6f6-d5369abd6389"), new Guid("956c4baf-e989-4046-b6cb-1d98df33cf6d"), "uk-UA", "Інше" },
                { new Guid("bfd40057-71c0-4f07-bd54-070fb158d86e"), new Guid("dd2bf352-9cfd-4b88-b46f-08217104f90d"), "uk-UA", "Пригоди" },
                { new Guid("cc24ce33-7808-404c-abe2-8618a173ea73"), new Guid("e96511b9-e864-44cc-899f-8313609ffb63"), "uk-UA", "Екшн" },
                { new Guid("e3bdad9c-e738-43d9-a9e3-a13adc028709"), new Guid("b3562f18-5e7c-411a-ab76-b675b78bd23d"), "uk-UA", "Гонки" },
                { new Guid("fa678f2e-e047-4de2-892a-b73680db0c39"), new Guid("927cc631-c4f1-4feb-8a0c-9db6cd43402a"), "uk-UA", "Формула" },
            });

        migrationBuilder.InsertData(
            table: "PlatformTranslation",
            columns: new[] { "Id", "CoreId", "LanguageCode", "Type" },
            values: new object[,]
            {
                { new Guid("8febf7f4-795a-47ee-adc9-47835bd2c906"), new Guid("83262eb9-517e-4581-b7ba-88b57c33d721"), "uk-UA", "Консоль" },
                { new Guid("bffd53a2-ccbd-44dd-834f-8da69ef0ef44"), new Guid("4adb2c38-f819-43cd-aa78-f46d482ceeb7"), "uk-UA", "Комп'ютер" },
                { new Guid("d6b4239d-7ddf-4eba-9c9e-726b5c36a550"), new Guid("467762c6-8a10-4570-b829-e29de78a0757"), "uk-UA", "Хмара" },
                { new Guid("e30f48c6-e381-48b0-8de8-85e0e40d688e"), new Guid("a9f806b4-28c5-4d7b-a776-65dfe029de8f"), "uk-UA", "Мобільні пристрої" },
            });

        migrationBuilder.CreateIndex(
            name: "IX_GenreTranslation_CoreId",
            table: "GenreTranslation",
            column: "CoreId");

        migrationBuilder.CreateIndex(
            name: "IX_PlatformTranslation_CoreId",
            table: "PlatformTranslation",
            column: "CoreId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "GenreTranslation");

        migrationBuilder.DropTable(
            name: "PlatformTranslation");
    }
}
