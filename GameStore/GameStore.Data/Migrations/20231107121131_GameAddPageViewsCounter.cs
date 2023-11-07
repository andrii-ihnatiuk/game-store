using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class GameAddPageViewsCounter : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<decimal>(
            name: "PageViews",
            table: "Games",
            type: "decimal(20,0)",
            nullable: false,
            defaultValue: 0m);

        migrationBuilder.UpdateData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("352997f0-9cb6-4951-8b55-10df09d2e168"),
            column: "PageViews",
            value: 0m);

        migrationBuilder.UpdateData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("4b5f1e22-cd59-4523-a4e9-f0c0239ab820"),
            column: "PageViews",
            value: 0m);

        migrationBuilder.UpdateData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("8e9d1000-50e0-4bd8-8159-42c7431f32b5"),
            column: "PageViews",
            value: 0m);

        migrationBuilder.UpdateData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("95ffb14c-267a-432a-9d7c-22f887290d49"),
            column: "PageViews",
            value: 0m);

        migrationBuilder.UpdateData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("e2e928c4-ab49-4bc0-a904-37c34e1385cc"),
            column: "PageViews",
            value: 0m);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "PageViews",
            table: "Games");
    }
}
