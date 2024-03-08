using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class SeedSomeImages : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "AppImage",
            columns: new[] { "Id", "GameId", "IsCover", "Large", "Order", "Small" },
            values: new object[,]
            {
                { new Guid("60ab69c0-571e-4255-9310-2281e64f81b6"), new Guid("8e9d1000-50e0-4bd8-8159-42c7431f32b5"), true, "https://andrii.blob.core.windows.net/gamestore-static/60ab69c0-571e-4255-9310-2281e64f81b6_large.jpg", 0, "https://andrii.blob.core.windows.net/gamestore-static/60ab69c0-571e-4255-9310-2281e64f81b6_small.jpg" },
                { new Guid("f95594bd-72ba-4d76-8e36-486458c42430"), new Guid("8e9d1000-50e0-4bd8-8159-42c7431f32b5"), false, "https://andrii.blob.core.windows.net/gamestore-static/f95594bd-72ba-4d76-8e36-486458c42430_large.jpg", 1, "https://andrii.blob.core.windows.net/gamestore-static/f95594bd-72ba-4d76-8e36-486458c42430_small.jpg" },
            });

        migrationBuilder.UpdateData(
            table: "Game",
            keyColumn: "Id",
            keyValue: new Guid("8e9d1000-50e0-4bd8-8159-42c7431f32b5"),
            column: "PreviewImgUrl",
            value: "https://andrii.blob.core.windows.net/gamestore-static/60ab69c0-571e-4255-9310-2281e64f81b6_small.jpg");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "AppImage",
            keyColumn: "Id",
            keyValue: new Guid("60ab69c0-571e-4255-9310-2281e64f81b6"));

        migrationBuilder.DeleteData(
            table: "AppImage",
            keyColumn: "Id",
            keyValue: new Guid("f95594bd-72ba-4d76-8e36-486458c42430"));

        migrationBuilder.UpdateData(
            table: "Game",
            keyColumn: "Id",
            keyValue: new Guid("8e9d1000-50e0-4bd8-8159-42c7431f32b5"),
            column: "PreviewImgUrl",
            value: null);
    }
}
