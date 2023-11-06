using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class GameAddTimestamps : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "CreationDate",
            table: "Games",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<DateTime>(
            name: "PublishDate",
            table: "Games",
            type: "datetime2",
            nullable: true);

        migrationBuilder.UpdateData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("352997f0-9cb6-4951-8b55-10df09d2e168"),
            columns: new[] { "CreationDate", "PublishDate" },
            values: new object[] { new DateTime(2023, 10, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2019, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc) });

        migrationBuilder.UpdateData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("4b5f1e22-cd59-4523-a4e9-f0c0239ab820"),
            columns: new[] { "CreationDate", "PublishDate" },
            values: new object[] { new DateTime(2023, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2014, 4, 11, 0, 0, 0, 0, DateTimeKind.Utc) });

        migrationBuilder.UpdateData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("8e9d1000-50e0-4bd8-8159-42c7431f32b5"),
            columns: new[] { "CreationDate", "PublishDate" },
            values: new object[] { new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2017, 4, 23, 0, 0, 0, 0, DateTimeKind.Utc) });

        migrationBuilder.UpdateData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("95ffb14c-267a-432a-9d7c-22f887290d49"),
            columns: new[] { "CreationDate", "PublishDate" },
            values: new object[] { new DateTime(2023, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2013, 9, 17, 0, 0, 0, 0, DateTimeKind.Utc) });

        migrationBuilder.UpdateData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("e2e928c4-ab49-4bc0-a904-37c34e1385cc"),
            columns: new[] { "CreationDate", "PublishDate" },
            values: new object[] { new DateTime(2023, 10, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc) });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CreationDate",
            table: "Games");

        migrationBuilder.DropColumn(
            name: "PublishDate",
            table: "Games");
    }
}
