using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class AddNotificationMethodEntity : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "CustomerId",
            table: "Order",
            type: "nvarchar(450)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.CreateTable(
            name: "NotificationMethod",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                NormalizedName = table.Column<string>(type: "nvarchar(450)", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_NotificationMethod", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "UserNotificationMethod",
            columns: table => new
            {
                UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                NotificationMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserNotificationMethod", x => new { x.UserId, x.NotificationMethodId });
                table.ForeignKey(
                    name: "FK_UserNotificationMethod_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_UserNotificationMethod_NotificationMethod_NotificationMethodId",
                    column: x => x.NotificationMethodId,
                    principalTable: "NotificationMethod",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "NotificationMethod",
            columns: new[] { "Id", "Name", "NormalizedName" },
            values: new object[,]
            {
                { new Guid("2754466f-78c0-4e64-ab1d-3e5287afede0"), "SMS", "SMS" },
                { new Guid("306632e8-8f2b-48d0-9e4d-bdb56abc05b0"), "Push", "PUSH" },
                { new Guid("f5e9772d-a3eb-4d19-b53d-975407624360"), "Email", "EMAIL" },
            });

        migrationBuilder.CreateIndex(
            name: "IX_Order_CustomerId",
            table: "Order",
            column: "CustomerId");

        migrationBuilder.CreateIndex(
            name: "IX_NotificationMethod_NormalizedName",
            table: "NotificationMethod",
            column: "NormalizedName",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_UserNotificationMethod_NotificationMethodId",
            table: "UserNotificationMethod",
            column: "NotificationMethodId");

        migrationBuilder.AddForeignKey(
            name: "FK_Order_AspNetUsers_CustomerId",
            table: "Order",
            column: "CustomerId",
            principalTable: "AspNetUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Order_AspNetUsers_CustomerId",
            table: "Order");

        migrationBuilder.DropTable(
            name: "UserNotificationMethod");

        migrationBuilder.DropTable(
            name: "NotificationMethod");

        migrationBuilder.DropIndex(
            name: "IX_Order_CustomerId",
            table: "Order");

        migrationBuilder.AlterColumn<string>(
            name: "CustomerId",
            table: "Order",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)");
    }
}
