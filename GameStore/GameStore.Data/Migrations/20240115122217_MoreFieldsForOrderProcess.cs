using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class MoreFieldsForOrderProcess : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "StrategyName",
            table: "PaymentMethod",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AlterColumn<int>(
            name: "Quantity",
            table: "OrderDetail",
            type: "int",
            nullable: false,
            oldClrType: typeof(short),
            oldType: "smallint");

        migrationBuilder.AlterColumn<int>(
            name: "Discount",
            table: "OrderDetail",
            type: "int",
            nullable: false,
            oldClrType: typeof(float),
            oldType: "real");

        migrationBuilder.AddColumn<decimal>(
            name: "FinalPrice",
            table: "OrderDetail",
            type: "decimal(18,2)",
            nullable: false,
            defaultValue: 0m);

        migrationBuilder.AddColumn<int>(
            name: "Discount",
            table: "Game",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.UpdateData(
            table: "Game",
            keyColumn: "Id",
            keyValue: new Guid("352997f0-9cb6-4951-8b55-10df09d2e168"),
            column: "Discount",
            value: 0);

        migrationBuilder.UpdateData(
            table: "Game",
            keyColumn: "Id",
            keyValue: new Guid("4b5f1e22-cd59-4523-a4e9-f0c0239ab820"),
            column: "Discount",
            value: 0);

        migrationBuilder.UpdateData(
            table: "Game",
            keyColumn: "Id",
            keyValue: new Guid("8e9d1000-50e0-4bd8-8159-42c7431f32b5"),
            column: "Discount",
            value: 10);

        migrationBuilder.UpdateData(
            table: "Game",
            keyColumn: "Id",
            keyValue: new Guid("95ffb14c-267a-432a-9d7c-22f887290d49"),
            column: "Discount",
            value: 0);

        migrationBuilder.UpdateData(
            table: "Game",
            keyColumn: "Id",
            keyValue: new Guid("e2e928c4-ab49-4bc0-a904-37c34e1385cc"),
            column: "Discount",
            value: 0);

        migrationBuilder.UpdateData(
            table: "PaymentMethod",
            keyColumn: "Id",
            keyValue: new Guid("32bda162-d288-4a60-a684-9bd7caf61951"),
            column: "StrategyName",
            value: 1);

        migrationBuilder.UpdateData(
            table: "PaymentMethod",
            keyColumn: "Id",
            keyValue: new Guid("77301abc-0738-4540-aa3a-19db9f6bc2dc"),
            column: "StrategyName",
            value: 0);

        migrationBuilder.UpdateData(
            table: "PaymentMethod",
            keyColumn: "Id",
            keyValue: new Guid("d84def54-1f51-4f1d-aedc-fc1d18b4fa12"),
            column: "StrategyName",
            value: 2);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "StrategyName",
            table: "PaymentMethod");

        migrationBuilder.DropColumn(
            name: "FinalPrice",
            table: "OrderDetail");

        migrationBuilder.DropColumn(
            name: "Discount",
            table: "Game");

        migrationBuilder.AlterColumn<short>(
            name: "Quantity",
            table: "OrderDetail",
            type: "smallint",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AlterColumn<float>(
            name: "Discount",
            table: "OrderDetail",
            type: "real",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");
    }
}
