using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class GameSoftDeleteSupport : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 9);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 20);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 22);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 23);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 24);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 26);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 27);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 28);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 30);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 32);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 33);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 34);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 35);

        migrationBuilder.AddColumn<bool>(
            name: "Deleted",
            table: "Game",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.UpdateData(
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: "45083adf-a7ca-4576-a374-d9a072ef0bda",
            column: "ConcurrencyStamp",
            value: "b149eec2-f18e-4acb-8c4c-4bb0e4c82e8b");

        migrationBuilder.UpdateData(
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: "7a44e3b0-3baf-4831-8362-1a32f7460051",
            column: "ConcurrencyStamp",
            value: "2cdbf1d1-f008-4fb5-a2cb-11b621d44a71");

        migrationBuilder.UpdateData(
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: "80e3704c-7e3d-48f3-bb14-dd8e996daa59",
            column: "ConcurrencyStamp",
            value: "bcec8e70-4b32-4ee1-b74e-5c555013aa29");

        migrationBuilder.UpdateData(
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: "b17220dc-9059-4daa-8048-af443aa08a78",
            column: "ConcurrencyStamp",
            value: "146b7446-fe49-40bf-9bbb-f1cb2fcc039e");

        migrationBuilder.UpdateData(
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: "d18422a3-6a29-44e9-88b2-774db642bf6d",
            column: "ConcurrencyStamp",
            value: "2581396a-ec75-4c4a-b90d-f7de670455ac");

        migrationBuilder.UpdateData(
            table: "AspNetUsers",
            keyColumn: "Id",
            keyValue: "91193760-0bd1-4eac-92f8-0bd8b4ac2c6a",
            columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
            values: new object[] { "949d2829-0c95-4e3c-812f-d48093a7c7fd", "AQAAAAEAACcQAAAAEL85uhHv75LYDizv0bc7p+eb0vNckMJiQ+6FkT1pntP/xm7CLDweHcd3s7p3Z8f1fg==", "f68dad0e-f850-4969-a4a3-b513064cb38f" });

        migrationBuilder.UpdateData(
            table: "Game",
            keyColumn: "Id",
            keyValue: new Guid("352997f0-9cb6-4951-8b55-10df09d2e168"),
            column: "Deleted",
            value: false);

        migrationBuilder.UpdateData(
            table: "Game",
            keyColumn: "Id",
            keyValue: new Guid("4b5f1e22-cd59-4523-a4e9-f0c0239ab820"),
            column: "Deleted",
            value: false);

        migrationBuilder.UpdateData(
            table: "Game",
            keyColumn: "Id",
            keyValue: new Guid("8e9d1000-50e0-4bd8-8159-42c7431f32b5"),
            column: "Deleted",
            value: false);

        migrationBuilder.UpdateData(
            table: "Game",
            keyColumn: "Id",
            keyValue: new Guid("95ffb14c-267a-432a-9d7c-22f887290d49"),
            column: "Deleted",
            value: false);

        migrationBuilder.UpdateData(
            table: "Game",
            keyColumn: "Id",
            keyValue: new Guid("e2e928c4-ab49-4bc0-a904-37c34e1385cc"),
            column: "Deleted",
            value: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Deleted",
            table: "Game");

        migrationBuilder.InsertData(
            table: "AspNetRoleClaims",
            columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
            values: new object[,]
            {
                { 9, "permission", "game:view", "80e3704c-7e3d-48f3-bb14-dd8e996daa59" },
                { 20, "permission", "game:view", "b17220dc-9059-4daa-8048-af443aa08a78" },
                { 22, "permission", "genre:view", "b17220dc-9059-4daa-8048-af443aa08a78" },
                { 23, "permission", "publisher:view", "b17220dc-9059-4daa-8048-af443aa08a78" },
                { 24, "permission", "platform:view", "b17220dc-9059-4daa-8048-af443aa08a78" },
                { 26, "permission", "game:view", "7a44e3b0-3baf-4831-8362-1a32f7460051" },
                { 27, "permission", "genre:view", "7a44e3b0-3baf-4831-8362-1a32f7460051" },
                { 28, "permission", "publisher:view", "7a44e3b0-3baf-4831-8362-1a32f7460051" },
                { 30, "permission", "platform:view", "7a44e3b0-3baf-4831-8362-1a32f7460051" },
                { 32, "permission", "game:view", "d18422a3-6a29-44e9-88b2-774db642bf6d" },
                { 33, "permission", "genre:view", "d18422a3-6a29-44e9-88b2-774db642bf6d" },
                { 34, "permission", "publisher:view", "d18422a3-6a29-44e9-88b2-774db642bf6d" },
                { 35, "permission", "platform:view", "d18422a3-6a29-44e9-88b2-774db642bf6d" },
            });

        migrationBuilder.UpdateData(
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: "45083adf-a7ca-4576-a374-d9a072ef0bda",
            column: "ConcurrencyStamp",
            value: "bab24dbb-a11d-48d1-b5f4-775b8fe021c1");

        migrationBuilder.UpdateData(
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: "7a44e3b0-3baf-4831-8362-1a32f7460051",
            column: "ConcurrencyStamp",
            value: "25393248-f79f-42f5-97c8-526f0b002964");

        migrationBuilder.UpdateData(
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: "80e3704c-7e3d-48f3-bb14-dd8e996daa59",
            column: "ConcurrencyStamp",
            value: "8b6a0d48-d770-4383-831b-3c2248e676e4");

        migrationBuilder.UpdateData(
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: "b17220dc-9059-4daa-8048-af443aa08a78",
            column: "ConcurrencyStamp",
            value: "af759983-120f-43de-b3e6-2b95fc9bcaef");

        migrationBuilder.UpdateData(
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: "d18422a3-6a29-44e9-88b2-774db642bf6d",
            column: "ConcurrencyStamp",
            value: "599c4d4e-2526-4dbb-97bb-d5ff8dc48adc");

        migrationBuilder.UpdateData(
            table: "AspNetUsers",
            keyColumn: "Id",
            keyValue: "91193760-0bd1-4eac-92f8-0bd8b4ac2c6a",
            columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
            values: new object[] { "43664a57-45c7-45d0-9c4c-ed255fedf99b", "AQAAAAEAACcQAAAAEIjQUdQ4HU3hNfH2Mt0YeqxNQD1hCBrd0eU0+zUUKliNpA+dAdLsQrG8ozm+0Rgfsg==", "5542687a-7128-4b7a-9972-c204c5a124f8" });
    }
}
