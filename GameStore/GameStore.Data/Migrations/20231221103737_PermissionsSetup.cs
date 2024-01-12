using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class PermissionsSetup : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "AspNetRoles",
            columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            values: new object[,]
            {
                { "45083adf-a7ca-4576-a374-d9a072ef0bda", "bab24dbb-a11d-48d1-b5f4-775b8fe021c1", "Administrator", "ADMINISTRATOR" },
                { "7a44e3b0-3baf-4831-8362-1a32f7460051", "25393248-f79f-42f5-97c8-526f0b002964", "Publisher", "PUBLISHER" },
                { "80e3704c-7e3d-48f3-bb14-dd8e996daa59", "8b6a0d48-d770-4383-831b-3c2248e676e4", "Manager", "MANAGER" },
                { "b17220dc-9059-4daa-8048-af443aa08a78", "af759983-120f-43de-b3e6-2b95fc9bcaef", "Moderator", "MODERATOR" },
                { "d18422a3-6a29-44e9-88b2-774db642bf6d", "599c4d4e-2526-4dbb-97bb-d5ff8dc48adc", "User", "USER" },
            });

        migrationBuilder.InsertData(
            table: "AspNetUsers",
            columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
            values: new object[] { "91193760-0bd1-4eac-92f8-0bd8b4ac2c6a", 0, "43664a57-45c7-45d0-9c4c-ed255fedf99b", null, false, false, null, null, "ADMIN", "AQAAAAEAACcQAAAAEIjQUdQ4HU3hNfH2Mt0YeqxNQD1hCBrd0eU0+zUUKliNpA+dAdLsQrG8ozm+0Rgfsg==", null, false, "5542687a-7128-4b7a-9972-c204c5a124f8", false, "Admin" });

        migrationBuilder.InsertData(
            table: "AspNetRoleClaims",
            columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
            values: new object[,]
            {
                { 1, "permission", "user:full", "45083adf-a7ca-4576-a374-d9a072ef0bda" },
                { 2, "permission", "role:full", "45083adf-a7ca-4576-a374-d9a072ef0bda" },
                { 3, "permission", "game:full", "45083adf-a7ca-4576-a374-d9a072ef0bda" },
                { 4, "permission", "genre:full", "45083adf-a7ca-4576-a374-d9a072ef0bda" },
                { 5, "permission", "publisher:full", "45083adf-a7ca-4576-a374-d9a072ef0bda" },
                { 6, "permission", "platform:full", "45083adf-a7ca-4576-a374-d9a072ef0bda" },
                { 7, "permission", "comment:full", "45083adf-a7ca-4576-a374-d9a072ef0bda" },
                { 8, "permission", "order:full", "45083adf-a7ca-4576-a374-d9a072ef0bda" },
                { 9, "permission", "game:view", "80e3704c-7e3d-48f3-bb14-dd8e996daa59" },
                { 10, "permission", "game:view-deleted", "80e3704c-7e3d-48f3-bb14-dd8e996daa59" },
                { 11, "permission", "game:create", "80e3704c-7e3d-48f3-bb14-dd8e996daa59" },
                { 12, "permission", "game:update", "80e3704c-7e3d-48f3-bb14-dd8e996daa59" },
                { 13, "permission", "game:delete", "80e3704c-7e3d-48f3-bb14-dd8e996daa59" },
                { 14, "permission", "genre:full", "80e3704c-7e3d-48f3-bb14-dd8e996daa59" },
                { 15, "permission", "publisher:full", "80e3704c-7e3d-48f3-bb14-dd8e996daa59" },
                { 16, "permission", "platform:full", "80e3704c-7e3d-48f3-bb14-dd8e996daa59" },
                { 17, "permission", "order:view-history", "80e3704c-7e3d-48f3-bb14-dd8e996daa59" },
                { 18, "permission", "order:update", "80e3704c-7e3d-48f3-bb14-dd8e996daa59" },
                { 19, "permission", "comment:create", "80e3704c-7e3d-48f3-bb14-dd8e996daa59" },
                { 20, "permission", "game:view", "b17220dc-9059-4daa-8048-af443aa08a78" },
                { 21, "permission", "game:view-deleted", "b17220dc-9059-4daa-8048-af443aa08a78" },
                { 22, "permission", "genre:view", "b17220dc-9059-4daa-8048-af443aa08a78" },
                { 23, "permission", "publisher:view", "b17220dc-9059-4daa-8048-af443aa08a78" },
                { 24, "permission", "platform:view", "b17220dc-9059-4daa-8048-af443aa08a78" },
                { 25, "permission", "comment:full", "b17220dc-9059-4daa-8048-af443aa08a78" },
                { 26, "permission", "game:view", "7a44e3b0-3baf-4831-8362-1a32f7460051" },
                { 27, "permission", "genre:view", "7a44e3b0-3baf-4831-8362-1a32f7460051" },
                { 28, "permission", "publisher:view", "7a44e3b0-3baf-4831-8362-1a32f7460051" },
                { 29, "permission", "publisher:update", "7a44e3b0-3baf-4831-8362-1a32f7460051" },
                { 30, "permission", "platform:view", "7a44e3b0-3baf-4831-8362-1a32f7460051" },
                { 31, "permission", "comment:create", "7a44e3b0-3baf-4831-8362-1a32f7460051" },
                { 32, "permission", "game:view", "d18422a3-6a29-44e9-88b2-774db642bf6d" },
                { 33, "permission", "genre:view", "d18422a3-6a29-44e9-88b2-774db642bf6d" },
                { 34, "permission", "publisher:view", "d18422a3-6a29-44e9-88b2-774db642bf6d" },
                { 35, "permission", "platform:view", "d18422a3-6a29-44e9-88b2-774db642bf6d" },
                { 36, "permission", "comment:create", "d18422a3-6a29-44e9-88b2-774db642bf6d" },
            });

        migrationBuilder.InsertData(
            table: "AspNetUserRoles",
            columns: new[] { "RoleId", "UserId" },
            values: new object[] { "45083adf-a7ca-4576-a374-d9a072ef0bda", "91193760-0bd1-4eac-92f8-0bd8b4ac2c6a" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 1);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 2);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 3);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 4);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 5);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 6);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 7);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 8);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 9);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 10);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 11);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 12);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 13);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 14);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 15);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 16);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 17);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 18);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 19);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 20);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 21);

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
            keyValue: 25);

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
            keyValue: 29);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 30);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 31);

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

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 36);

        migrationBuilder.DeleteData(
            table: "AspNetUserRoles",
            keyColumns: new[] { "RoleId", "UserId" },
            keyValues: new object[] { "45083adf-a7ca-4576-a374-d9a072ef0bda", "91193760-0bd1-4eac-92f8-0bd8b4ac2c6a" });

        migrationBuilder.DeleteData(
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: "45083adf-a7ca-4576-a374-d9a072ef0bda");

        migrationBuilder.DeleteData(
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: "7a44e3b0-3baf-4831-8362-1a32f7460051");

        migrationBuilder.DeleteData(
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: "80e3704c-7e3d-48f3-bb14-dd8e996daa59");

        migrationBuilder.DeleteData(
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: "b17220dc-9059-4daa-8048-af443aa08a78");

        migrationBuilder.DeleteData(
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: "d18422a3-6a29-44e9-88b2-774db642bf6d");

        migrationBuilder.DeleteData(
            table: "AspNetUsers",
            keyColumn: "Id",
            keyValue: "91193760-0bd1-4eac-92f8-0bd8b4ac2c6a");
    }
}
