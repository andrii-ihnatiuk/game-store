using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class AddImagesForGame : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "PreviewImgUrl",
            table: "Game",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "AppImage",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Large = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Small = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IsCover = table.Column<bool>(type: "bit", nullable: false),
                Order = table.Column<int>(type: "int", nullable: false),
                GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AppImage", x => x.Id);
                table.ForeignKey(
                    name: "FK_AppImage_Game_GameId",
                    column: x => x.GameId,
                    principalTable: "Game",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.UpdateData(
            table: "Game",
            keyColumn: "Id",
            keyValue: new Guid("352997f0-9cb6-4951-8b55-10df09d2e168"),
            column: "PreviewImgUrl",
            value: null);

        migrationBuilder.UpdateData(
            table: "Game",
            keyColumn: "Id",
            keyValue: new Guid("4b5f1e22-cd59-4523-a4e9-f0c0239ab820"),
            column: "PreviewImgUrl",
            value: null);

        migrationBuilder.UpdateData(
            table: "Game",
            keyColumn: "Id",
            keyValue: new Guid("8e9d1000-50e0-4bd8-8159-42c7431f32b5"),
            column: "PreviewImgUrl",
            value: null);

        migrationBuilder.UpdateData(
            table: "Game",
            keyColumn: "Id",
            keyValue: new Guid("95ffb14c-267a-432a-9d7c-22f887290d49"),
            column: "PreviewImgUrl",
            value: null);

        migrationBuilder.UpdateData(
            table: "Game",
            keyColumn: "Id",
            keyValue: new Guid("e2e928c4-ab49-4bc0-a904-37c34e1385cc"),
            column: "PreviewImgUrl",
            value: null);

        migrationBuilder.CreateIndex(
            name: "IX_AppImage_GameId",
            table: "AppImage",
            column: "GameId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AppImage");

        migrationBuilder.DropColumn(
            name: "PreviewImgUrl",
            table: "Game");
    }
}
