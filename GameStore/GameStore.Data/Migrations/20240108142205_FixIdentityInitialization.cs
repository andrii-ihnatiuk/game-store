using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class FixIdentityInitialization : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
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
            keyValue: 21);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 25);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 29);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 30);

        migrationBuilder.DeleteData(
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: "d18422a3-6a29-44e9-88b2-774db642bf6d");

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
            table: "AspNetUsers",
            keyColumn: "Id",
            keyValue: "91193760-0bd1-4eac-92f8-0bd8b4ac2c6a");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "AspNetRoles",
            columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            values: new object[,]
            {
                { "45083adf-a7ca-4576-a374-d9a072ef0bda", "b149eec2-f18e-4acb-8c4c-4bb0e4c82e8b", "Administrator", "ADMINISTRATOR" },
                { "7a44e3b0-3baf-4831-8362-1a32f7460051", "2cdbf1d1-f008-4fb5-a2cb-11b621d44a71", "Publisher", "PUBLISHER" },
                { "80e3704c-7e3d-48f3-bb14-dd8e996daa59", "bcec8e70-4b32-4ee1-b74e-5c555013aa29", "Manager", "MANAGER" },
                { "b17220dc-9059-4daa-8048-af443aa08a78", "146b7446-fe49-40bf-9bbb-f1cb2fcc039e", "Moderator", "MODERATOR" },
                { "d18422a3-6a29-44e9-88b2-774db642bf6d", "2581396a-ec75-4c4a-b90d-f7de670455ac", "User", "USER" },
            });

        migrationBuilder.InsertData(
            table: "AspNetUsers",
            columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
            values: new object[] { "91193760-0bd1-4eac-92f8-0bd8b4ac2c6a", 0, "949d2829-0c95-4e3c-812f-d48093a7c7fd", null, false, false, null, null, "ADMIN", "AQAAAAEAACcQAAAAEL85uhHv75LYDizv0bc7p+eb0vNckMJiQ+6FkT1pntP/xm7CLDweHcd3s7p3Z8f1fg==", null, false, "f68dad0e-f850-4969-a4a3-b513064cb38f", false, "Admin" });

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
                { 10, "permission", "game:view-deleted", "80e3704c-7e3d-48f3-bb14-dd8e996daa59" },
                { 11, "permission", "game:create", "80e3704c-7e3d-48f3-bb14-dd8e996daa59" },
                { 12, "permission", "game:update", "80e3704c-7e3d-48f3-bb14-dd8e996daa59" },
                { 13, "permission", "game:delete", "80e3704c-7e3d-48f3-bb14-dd8e996daa59" },
                { 14, "permission", "genre:full", "80e3704c-7e3d-48f3-bb14-dd8e996daa59" },
                { 15, "permission", "publisher:full", "80e3704c-7e3d-48f3-bb14-dd8e996daa59" },
                { 16, "permission", "platform:full", "80e3704c-7e3d-48f3-bb14-dd8e996daa59" },
                { 17, "permission", "order:view-history", "80e3704c-7e3d-48f3-bb14-dd8e996daa59" },
                { 18, "permission", "order:update", "80e3704c-7e3d-48f3-bb14-dd8e996daa59" },
                { 21, "permission", "game:view-deleted", "b17220dc-9059-4daa-8048-af443aa08a78" },
                { 25, "permission", "comment:full", "b17220dc-9059-4daa-8048-af443aa08a78" },
                { 29, "permission", "publisher:update-self", "7a44e3b0-3baf-4831-8362-1a32f7460051" },
                { 30, "permission", "game:update-own", "7a44e3b0-3baf-4831-8362-1a32f7460051" },
            });

        migrationBuilder.InsertData(
            table: "AspNetUserRoles",
            columns: new[] { "RoleId", "UserId" },
            values: new object[] { "45083adf-a7ca-4576-a374-d9a072ef0bda", "91193760-0bd1-4eac-92f8-0bd8b4ac2c6a" });
    }
}
