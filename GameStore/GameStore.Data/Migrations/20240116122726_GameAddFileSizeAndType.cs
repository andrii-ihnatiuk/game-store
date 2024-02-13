using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class GameAddFileSizeAndType : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<decimal>(
            name: "FinalPrice",
            table: "OrderDetail",
            type: "money",
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,2)");

        migrationBuilder.AddColumn<string>(
            name: "FileSize",
            table: "Game",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Type",
            table: "Game",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.UpdateData(
            table: "Game",
            keyColumn: "Id",
            keyValue: new Guid("352997f0-9cb6-4951-8b55-10df09d2e168"),
            columns: new[] { "FileSize", "Type" },
            values: new object[] { null, "Bundle" });

        migrationBuilder.UpdateData(
            table: "Game",
            keyColumn: "Id",
            keyValue: new Guid("4b5f1e22-cd59-4523-a4e9-f0c0239ab820"),
            columns: new[] { "FileSize", "Type" },
            values: new object[] { null, "Full Game" });

        migrationBuilder.UpdateData(
            table: "Game",
            keyColumn: "Id",
            keyValue: new Guid("8e9d1000-50e0-4bd8-8159-42c7431f32b5"),
            columns: new[] { "FileSize", "Type" },
            values: new object[] { "65.76 GB", "Full Game" });

        migrationBuilder.UpdateData(
            table: "Game",
            keyColumn: "Id",
            keyValue: new Guid("95ffb14c-267a-432a-9d7c-22f887290d49"),
            columns: new[] { "FileSize", "Type" },
            values: new object[] { "120 GB", "Bundle" });

        migrationBuilder.UpdateData(
            table: "Game",
            keyColumn: "Id",
            keyValue: new Guid("e2e928c4-ab49-4bc0-a904-37c34e1385cc"),
            columns: new[] { "Discount", "FileSize", "Type" },
            values: new object[] { 5, "54.32 GB", "Collector's edition" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "FileSize",
            table: "Game");

        migrationBuilder.DropColumn(
            name: "Type",
            table: "Game");

        migrationBuilder.AlterColumn<decimal>(
            name: "FinalPrice",
            table: "OrderDetail",
            type: "decimal(18,2)",
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "money");

        migrationBuilder.UpdateData(
            table: "Game",
            keyColumn: "Id",
            keyValue: new Guid("e2e928c4-ab49-4bc0-a904-37c34e1385cc"),
            column: "Discount",
            value: 0);
    }
}
