using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class AddShippedDateToOrder : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "ShippedDate",
            table: "Order",
            type: "datetime2",
            nullable: true);

        migrationBuilder.UpdateData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 19,
            column: "ClaimValue",
            value: "comment:create-on-deleted");

        migrationBuilder.UpdateData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 31,
            column: "ClaimValue",
            value: "comment:create-on-deleted");

        migrationBuilder.UpdateData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 36,
            column: "ClaimValue",
            value: "comment:create-on-deleted");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "ShippedDate",
            table: "Order");

        migrationBuilder.UpdateData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 19,
            column: "ClaimValue",
            value: "comment:create");

        migrationBuilder.UpdateData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 31,
            column: "ClaimValue",
            value: "comment:create");

        migrationBuilder.UpdateData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 36,
            column: "ClaimValue",
            value: "comment:create");
    }
}
