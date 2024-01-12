using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class ConnectPublisherAndUser : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "AccountId",
            table: "Publisher",
            type: "nvarchar(450)",
            nullable: true);

        migrationBuilder.UpdateData(
            table: "Publisher",
            keyColumn: "Id",
            keyValue: new Guid("08029ea0-8bd8-494c-b3c5-b65a61538f81"),
            column: "AccountId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Publisher",
            keyColumn: "Id",
            keyValue: new Guid("3f8fc430-d0a5-4779-a3a4-5e0add54fde6"),
            column: "AccountId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Publisher",
            keyColumn: "Id",
            keyValue: new Guid("97fd0c5c-0504-4687-9a36-a65e699ca393"),
            column: "AccountId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Publisher",
            keyColumn: "Id",
            keyValue: new Guid("defd4ed1-a967-48af-83fb-4e5ffee412b0"),
            column: "AccountId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Publisher",
            keyColumn: "Id",
            keyValue: new Guid("ec62c5de-e415-4e74-bc75-3a7606563c78"),
            column: "AccountId",
            value: null);

        migrationBuilder.CreateIndex(
            name: "IX_Publisher_AccountId",
            table: "Publisher",
            column: "AccountId",
            unique: true,
            filter: "[AccountId] IS NOT NULL");

        migrationBuilder.AddForeignKey(
            name: "FK_Publisher_AspNetUsers_AccountId",
            table: "Publisher",
            column: "AccountId",
            principalTable: "AspNetUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Publisher_AspNetUsers_AccountId",
            table: "Publisher");

        migrationBuilder.DropIndex(
            name: "IX_Publisher_AccountId",
            table: "Publisher");

        migrationBuilder.DropColumn(
            name: "AccountId",
            table: "Publisher");
    }
}
