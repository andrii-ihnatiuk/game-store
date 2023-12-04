using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class ExtendEntities : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "LegacyId",
            table: "Publisher",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "LegacyId",
            table: "Genres",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Picture",
            table: "Genres",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "LegacyId",
            table: "Games",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.UpdateData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("352997f0-9cb6-4951-8b55-10df09d2e168"),
            column: "LegacyId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("4b5f1e22-cd59-4523-a4e9-f0c0239ab820"),
            column: "LegacyId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("8e9d1000-50e0-4bd8-8159-42c7431f32b5"),
            column: "LegacyId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("95ffb14c-267a-432a-9d7c-22f887290d49"),
            column: "LegacyId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("e2e928c4-ab49-4bc0-a904-37c34e1385cc"),
            column: "LegacyId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"),
            columns: new[] { "LegacyId", "Picture" },
            values: new object[] { null, null });

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("073f790e-a105-491d-965c-946e841c3b3e"),
            columns: new[] { "LegacyId", "Picture" },
            values: new object[] { null, null });

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("13ba20d2-42fc-4eaa-a7f9-900782cabfc3"),
            columns: new[] { "LegacyId", "Picture" },
            values: new object[] { null, null });

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("2ba0604c-5515-470c-a500-58b60e8be000"),
            columns: new[] { "LegacyId", "Picture" },
            values: new object[] { null, null });

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("5d7545f5-a77a-4632-89c6-40f8756d2f75"),
            columns: new[] { "LegacyId", "Picture" },
            values: new object[] { null, null });

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("5efe1909-c0b0-440e-8915-2831570816ec"),
            columns: new[] { "LegacyId", "Picture" },
            values: new object[] { null, null });

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("837e2c69-c602-4038-9754-47b9c005a0a8"),
            columns: new[] { "LegacyId", "Picture" },
            values: new object[] { null, null });

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("927cc631-c4f1-4feb-8a0c-9db6cd43402a"),
            columns: new[] { "LegacyId", "Picture" },
            values: new object[] { null, null });

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("956c4baf-e989-4046-b6cb-1d98df33cf6d"),
            columns: new[] { "LegacyId", "Picture" },
            values: new object[] { null, null });

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("b3562f18-5e7c-411a-ab76-b675b78bd23d"),
            columns: new[] { "LegacyId", "Picture" },
            values: new object[] { null, null });

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("d9eaef23-9680-4d25-869b-4ed91847fd03"),
            columns: new[] { "LegacyId", "Picture" },
            values: new object[] { null, null });

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("dd2bf352-9cfd-4b88-b46f-08217104f90d"),
            columns: new[] { "LegacyId", "Picture" },
            values: new object[] { null, null });

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("e0e725ed-50ed-4ff7-94e7-38d0e1d2fa39"),
            columns: new[] { "LegacyId", "Picture" },
            values: new object[] { null, null });

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("e96511b9-e864-44cc-899f-8313609ffb63"),
            columns: new[] { "LegacyId", "Picture" },
            values: new object[] { null, null });

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("ead8a2bf-e751-47a9-a2fc-e8b6f643b0c8"),
            columns: new[] { "LegacyId", "Picture" },
            values: new object[] { null, null });

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("f82e27c8-760e-4cb8-866f-60bb33caeaac"),
            columns: new[] { "LegacyId", "Picture" },
            values: new object[] { null, null });

        migrationBuilder.UpdateData(
            table: "Publisher",
            keyColumn: "Id",
            keyValue: new Guid("08029ea0-8bd8-494c-b3c5-b65a61538f81"),
            column: "LegacyId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Publisher",
            keyColumn: "Id",
            keyValue: new Guid("3f8fc430-d0a5-4779-a3a4-5e0add54fde6"),
            column: "LegacyId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Publisher",
            keyColumn: "Id",
            keyValue: new Guid("97fd0c5c-0504-4687-9a36-a65e699ca393"),
            column: "LegacyId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Publisher",
            keyColumn: "Id",
            keyValue: new Guid("defd4ed1-a967-48af-83fb-4e5ffee412b0"),
            column: "LegacyId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Publisher",
            keyColumn: "Id",
            keyValue: new Guid("ec62c5de-e415-4e74-bc75-3a7606563c78"),
            column: "LegacyId",
            value: null);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "LegacyId",
            table: "Publisher");

        migrationBuilder.DropColumn(
            name: "LegacyId",
            table: "Genres");

        migrationBuilder.DropColumn(
            name: "Picture",
            table: "Genres");

        migrationBuilder.DropColumn(
            name: "LegacyId",
            table: "Games");
    }
}
