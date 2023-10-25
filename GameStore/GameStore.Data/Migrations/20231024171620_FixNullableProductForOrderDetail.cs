using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class FixNullableProductForOrderDetail : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_OrderDetail_Games_ProductId",
            table: "OrderDetail");

        migrationBuilder.AlterColumn<Guid>(
            name: "ProductId",
            table: "OrderDetail",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.AddForeignKey(
            name: "FK_OrderDetail_Games_ProductId",
            table: "OrderDetail",
            column: "ProductId",
            principalTable: "Games",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_OrderDetail_Games_ProductId",
            table: "OrderDetail");

        migrationBuilder.AlterColumn<Guid>(
            name: "ProductId",
            table: "OrderDetail",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");

        migrationBuilder.AddForeignKey(
            name: "FK_OrderDetail_Games_ProductId",
            table: "OrderDetail",
            column: "ProductId",
            principalTable: "Games",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);
    }
}
