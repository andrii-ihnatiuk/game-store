using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class PermissionsFix : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 19);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 31);

        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 36);

        migrationBuilder.UpdateData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 29,
            column: "ClaimValue",
            value: "publisher:update-self");

        migrationBuilder.InsertData(
            table: "AspNetRoleClaims",
            columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
            values: new object[] { 30, "permission", "game:update-own", "7a44e3b0-3baf-4831-8362-1a32f7460051" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 30);

        migrationBuilder.UpdateData(
            table: "AspNetRoleClaims",
            keyColumn: "Id",
            keyValue: 29,
            column: "ClaimValue",
            value: "publisher:update");

        migrationBuilder.InsertData(
            table: "AspNetRoleClaims",
            columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
            values: new object[,]
            {
                { 19, "permission", "comment:create-on-deleted", "80e3704c-7e3d-48f3-bb14-dd8e996daa59" },
                { 31, "permission", "comment:create-on-deleted", "7a44e3b0-3baf-4831-8362-1a32f7460051" },
                { 36, "permission", "comment:create-on-deleted", "d18422a3-6a29-44e9-88b2-774db642bf6d" },
            });
    }
}
