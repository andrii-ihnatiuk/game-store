using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class AddTranslationForGame : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "GameTranslation",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GameTranslation", x => x.Id);
                table.ForeignKey(
                    name: "FK_GameTranslation_Game_CoreId",
                    column: x => x.CoreId,
                    principalTable: "Game",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "GameTranslation",
            columns: new[] { "Id", "CoreId", "Description", "LanguageCode", "Name", "Type" },
            values: new object[,]
            {
                { new Guid("1fe565b1-ac46-435a-9c10-ed4820a6a4a5"), new Guid("8e9d1000-50e0-4bd8-8159-42c7431f32b5"), "Пригодницька гра у відкритому світі.", "uk-UA", "The Legend of Zelda: Breath of the Wild / Легенда про Зельду", "Повна гра" },
                { new Guid("3baba5f2-85aa-4c18-a86a-7f02a5e037aa"), new Guid("95ffb14c-267a-432a-9d7c-22f887290d49"), "Пригодницька гра з відкритим світом.", "uk-UA", "Grand Theft Auto V / ГТА 5", "Комплект" },
                { new Guid("40c0e97f-3c51-4e09-ad59-ccc75515a01f"), new Guid("4b5f1e22-cd59-4523-a4e9-f0c0239ab820"), "Hearthstone — це швидка стратегічна карткова гра від Blizzard Entertainment.", "uk-UA", "Hearthstone / Хартстоун", "Повна гра" },
                { new Guid("b632f58b-f4b7-4d6b-bbcf-f2654011c044"), new Guid("e2e928c4-ab49-4bc0-a904-37c34e1385cc"), "Overwatch 2 — це безкоштовна командна гра-екшен.", "uk-UA", "Overwatch 2 / Овервотч", "Колекційне видання" },
            });

        migrationBuilder.CreateIndex(
            name: "IX_GameTranslation_CoreId",
            table: "GameTranslation",
            column: "CoreId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "GameTranslation");
    }
}
