using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class OrderAndOrderDetails : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "GameGenre",
            keyColumns: new[] { "GameId", "GenreId" },
            keyValues: new object[] { new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"), new Guid("e96511b9-e864-44cc-899f-8313609ffb63") });

        migrationBuilder.DeleteData(
            table: "GameGenre",
            keyColumns: new[] { "GameId", "GenreId" },
            keyValues: new object[] { new Guid("073f790e-a105-491d-965c-946e841c3b3e"), new Guid("dd2bf352-9cfd-4b88-b46f-08217104f90d") });

        migrationBuilder.DeleteData(
            table: "GameGenre",
            keyColumns: new[] { "GameId", "GenreId" },
            keyValues: new object[] { new Guid("073f790e-a105-491d-965c-946e841c3b3e"), new Guid("e96511b9-e864-44cc-899f-8313609ffb63") });

        migrationBuilder.DeleteData(
            table: "GamePlatform",
            keyColumns: new[] { "GameId", "PlatformId" },
            keyValues: new object[] { new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"), new Guid("5efe1909-c0b0-440e-8915-2831570816ec") });

        migrationBuilder.DeleteData(
            table: "GamePlatform",
            keyColumns: new[] { "GameId", "PlatformId" },
            keyValues: new object[] { new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"), new Guid("ead8a2bf-e751-47a9-a2fc-e8b6f643b0c8") });

        migrationBuilder.DeleteData(
            table: "GamePlatform",
            keyColumns: new[] { "GameId", "PlatformId" },
            keyValues: new object[] { new Guid("073f790e-a105-491d-965c-946e841c3b3e"), new Guid("5efe1909-c0b0-440e-8915-2831570816ec") });

        migrationBuilder.DeleteData(
            table: "Platform",
            keyColumn: "Id",
            keyValue: new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"));

        migrationBuilder.DeleteData(
            table: "Platform",
            keyColumn: "Id",
            keyValue: new Guid("073f790e-a105-491d-965c-946e841c3b3e"));

        migrationBuilder.DeleteData(
            table: "Publisher",
            keyColumn: "Id",
            keyValue: new Guid("ead8a2bf-e751-47a9-a2fc-e8b6f643b0c8"));

        migrationBuilder.DeleteData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"));

        migrationBuilder.DeleteData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("073f790e-a105-491d-965c-946e841c3b3e"));

        migrationBuilder.DeleteData(
            table: "Platform",
            keyColumn: "Id",
            keyValue: new Guid("5efe1909-c0b0-440e-8915-2831570816ec"));

        migrationBuilder.DeleteData(
            table: "Platform",
            keyColumn: "Id",
            keyValue: new Guid("ead8a2bf-e751-47a9-a2fc-e8b6f643b0c8"));

        migrationBuilder.DeleteData(
            table: "Publisher",
            keyColumn: "Id",
            keyValue: new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"));

        migrationBuilder.DeleteData(
            table: "Publisher",
            keyColumn: "Id",
            keyValue: new Guid("073f790e-a105-491d-965c-946e841c3b3e"));

        migrationBuilder.CreateTable(
            name: "Order",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                Sum = table.Column<decimal>(type: "money", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Order", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "OrderDetail",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Price = table.Column<decimal>(type: "money", nullable: false),
                Quantity = table.Column<short>(type: "smallint", nullable: false),
                Discount = table.Column<float>(type: "real", nullable: false),
                CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OrderDetail", x => x.Id);
                table.ForeignKey(
                    name: "FK_OrderDetail_Games_ProductId",
                    column: x => x.ProductId,
                    principalTable: "Games",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
                table.ForeignKey(
                    name: "FK_OrderDetail_Order_OrderId",
                    column: x => x.OrderId,
                    principalTable: "Order",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "Platform",
            columns: new[] { "Id", "Type" },
            values: new object[,]
            {
                { new Guid("467762c6-8a10-4570-b829-e29de78a0757"), "browser" },
                { new Guid("4adb2c38-f819-43cd-aa78-f46d482ceeb7"), "desktop" },
                { new Guid("83262eb9-517e-4581-b7ba-88b57c33d721"), "console" },
                { new Guid("a9f806b4-28c5-4d7b-a776-65dfe029de8f"), "mobile" },
            });

        migrationBuilder.InsertData(
            table: "Publisher",
            columns: new[] { "Id", "CompanyName", "Description", "HomePage" },
            values: new object[,]
            {
                { new Guid("08029ea0-8bd8-494c-b3c5-b65a61538f81"), "Electronic Arts", "Electronic Arts Inc. is a global leader in digital interactive entertainment.", "https://www.ea.com/" },
                { new Guid("3f8fc430-d0a5-4779-a3a4-5e0add54fde6"), "Activision", "Activision Publishing, Inc. is an American video game publisher based in Santa Monica, California.", "https://www.activision.com/" },
                { new Guid("97fd0c5c-0504-4687-9a36-a65e699ca393"), "Ubisoft", "Ubisoft Entertainment SA is a French video game company headquartered in Montreal.", "https://www.ubisoft.com/" },
            });

        migrationBuilder.InsertData(
            table: "Games",
            columns: new[] { "Id", "Alias", "Description", "Discontinued", "Name", "Price", "PublisherId", "UnitInStock" },
            values: new object[,]
            {
                { new Guid("8e9d1000-50e0-4bd8-8159-42c7431f32b5"), "zelda-breath-of-the-wild", "An action-adventure game in an open world.", false, "The Legend of Zelda: Breath of the Wild", 1500.2m, new Guid("3f8fc430-d0a5-4779-a3a4-5e0add54fde6"), (short)50 },
                { new Guid("95ffb14c-267a-432a-9d7c-22f887290d49"), "gta-v", "An open-world action-adventure game.", true, "Grand Theft Auto V", 500m, new Guid("08029ea0-8bd8-494c-b3c5-b65a61538f81"), (short)32 },
            });

        migrationBuilder.InsertData(
            table: "GameGenre",
            columns: new[] { "GameId", "GenreId" },
            values: new object[,]
            {
                { new Guid("8e9d1000-50e0-4bd8-8159-42c7431f32b5"), new Guid("dd2bf352-9cfd-4b88-b46f-08217104f90d") },
                { new Guid("8e9d1000-50e0-4bd8-8159-42c7431f32b5"), new Guid("e96511b9-e864-44cc-899f-8313609ffb63") },
                { new Guid("95ffb14c-267a-432a-9d7c-22f887290d49"), new Guid("e96511b9-e864-44cc-899f-8313609ffb63") },
            });

        migrationBuilder.InsertData(
            table: "GamePlatform",
            columns: new[] { "GameId", "PlatformId" },
            values: new object[,]
            {
                { new Guid("8e9d1000-50e0-4bd8-8159-42c7431f32b5"), new Guid("83262eb9-517e-4581-b7ba-88b57c33d721") },
                { new Guid("95ffb14c-267a-432a-9d7c-22f887290d49"), new Guid("4adb2c38-f819-43cd-aa78-f46d482ceeb7") },
                { new Guid("95ffb14c-267a-432a-9d7c-22f887290d49"), new Guid("83262eb9-517e-4581-b7ba-88b57c33d721") },
            });

        migrationBuilder.CreateIndex(
            name: "IX_Publisher_CompanyName",
            table: "Publisher",
            column: "CompanyName",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_OrderDetail_OrderId",
            table: "OrderDetail",
            column: "OrderId");

        migrationBuilder.CreateIndex(
            name: "IX_OrderDetail_ProductId",
            table: "OrderDetail",
            column: "ProductId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "OrderDetail");

        migrationBuilder.DropTable(
            name: "Order");

        migrationBuilder.DropIndex(
            name: "IX_Publisher_CompanyName",
            table: "Publisher");

        migrationBuilder.DeleteData(
            table: "GameGenre",
            keyColumns: new[] { "GameId", "GenreId" },
            keyValues: new object[] { new Guid("8e9d1000-50e0-4bd8-8159-42c7431f32b5"), new Guid("dd2bf352-9cfd-4b88-b46f-08217104f90d") });

        migrationBuilder.DeleteData(
            table: "GameGenre",
            keyColumns: new[] { "GameId", "GenreId" },
            keyValues: new object[] { new Guid("8e9d1000-50e0-4bd8-8159-42c7431f32b5"), new Guid("e96511b9-e864-44cc-899f-8313609ffb63") });

        migrationBuilder.DeleteData(
            table: "GameGenre",
            keyColumns: new[] { "GameId", "GenreId" },
            keyValues: new object[] { new Guid("95ffb14c-267a-432a-9d7c-22f887290d49"), new Guid("e96511b9-e864-44cc-899f-8313609ffb63") });

        migrationBuilder.DeleteData(
            table: "GamePlatform",
            keyColumns: new[] { "GameId", "PlatformId" },
            keyValues: new object[] { new Guid("8e9d1000-50e0-4bd8-8159-42c7431f32b5"), new Guid("83262eb9-517e-4581-b7ba-88b57c33d721") });

        migrationBuilder.DeleteData(
            table: "GamePlatform",
            keyColumns: new[] { "GameId", "PlatformId" },
            keyValues: new object[] { new Guid("95ffb14c-267a-432a-9d7c-22f887290d49"), new Guid("4adb2c38-f819-43cd-aa78-f46d482ceeb7") });

        migrationBuilder.DeleteData(
            table: "GamePlatform",
            keyColumns: new[] { "GameId", "PlatformId" },
            keyValues: new object[] { new Guid("95ffb14c-267a-432a-9d7c-22f887290d49"), new Guid("83262eb9-517e-4581-b7ba-88b57c33d721") });

        migrationBuilder.DeleteData(
            table: "Platform",
            keyColumn: "Id",
            keyValue: new Guid("467762c6-8a10-4570-b829-e29de78a0757"));

        migrationBuilder.DeleteData(
            table: "Platform",
            keyColumn: "Id",
            keyValue: new Guid("a9f806b4-28c5-4d7b-a776-65dfe029de8f"));

        migrationBuilder.DeleteData(
            table: "Publisher",
            keyColumn: "Id",
            keyValue: new Guid("97fd0c5c-0504-4687-9a36-a65e699ca393"));

        migrationBuilder.DeleteData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("8e9d1000-50e0-4bd8-8159-42c7431f32b5"));

        migrationBuilder.DeleteData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("95ffb14c-267a-432a-9d7c-22f887290d49"));

        migrationBuilder.DeleteData(
            table: "Platform",
            keyColumn: "Id",
            keyValue: new Guid("4adb2c38-f819-43cd-aa78-f46d482ceeb7"));

        migrationBuilder.DeleteData(
            table: "Platform",
            keyColumn: "Id",
            keyValue: new Guid("83262eb9-517e-4581-b7ba-88b57c33d721"));

        migrationBuilder.DeleteData(
            table: "Publisher",
            keyColumn: "Id",
            keyValue: new Guid("08029ea0-8bd8-494c-b3c5-b65a61538f81"));

        migrationBuilder.DeleteData(
            table: "Publisher",
            keyColumn: "Id",
            keyValue: new Guid("3f8fc430-d0a5-4779-a3a4-5e0add54fde6"));

        migrationBuilder.InsertData(
            table: "Platform",
            columns: new[] { "Id", "Type" },
            values: new object[,]
            {
                { new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"), "browser" },
                { new Guid("073f790e-a105-491d-965c-946e841c3b3e"), "mobile" },
                { new Guid("5efe1909-c0b0-440e-8915-2831570816ec"), "console" },
                { new Guid("ead8a2bf-e751-47a9-a2fc-e8b6f643b0c8"), "desktop" },
            });

        migrationBuilder.InsertData(
            table: "Publisher",
            columns: new[] { "Id", "CompanyName", "Description", "HomePage" },
            values: new object[,]
            {
                { new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"), "Electronic Arts", "Electronic Arts Inc. is a global leader in digital interactive entertainment.", "https://www.ea.com/" },
                { new Guid("073f790e-a105-491d-965c-946e841c3b3e"), "Activision", "Activision Publishing, Inc. is an American video game publisher based in Santa Monica, California.", "https://www.activision.com/" },
                { new Guid("ead8a2bf-e751-47a9-a2fc-e8b6f643b0c8"), "Ubisoft", "Ubisoft Entertainment SA is a French video game company headquartered in Montreal.", "https://www.ubisoft.com/" },
            });

        migrationBuilder.InsertData(
            table: "Games",
            columns: new[] { "Id", "Alias", "Description", "Discontinued", "Name", "Price", "PublisherId", "UnitInStock" },
            values: new object[,]
            {
                { new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"), "gta-v", "An open-world action-adventure game.", true, "Grand Theft Auto V", 500m, new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"), (short)32 },
                { new Guid("073f790e-a105-491d-965c-946e841c3b3e"), "zelda-breath-of-the-wild", "An action-adventure game in an open world.", false, "The Legend of Zelda: Breath of the Wild", 1500.2m, new Guid("073f790e-a105-491d-965c-946e841c3b3e"), (short)50 },
            });

        migrationBuilder.InsertData(
            table: "GameGenre",
            columns: new[] { "GameId", "GenreId" },
            values: new object[,]
            {
                { new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"), new Guid("e96511b9-e864-44cc-899f-8313609ffb63") },
                { new Guid("073f790e-a105-491d-965c-946e841c3b3e"), new Guid("dd2bf352-9cfd-4b88-b46f-08217104f90d") },
                { new Guid("073f790e-a105-491d-965c-946e841c3b3e"), new Guid("e96511b9-e864-44cc-899f-8313609ffb63") },
            });

        migrationBuilder.InsertData(
            table: "GamePlatform",
            columns: new[] { "GameId", "PlatformId" },
            values: new object[,]
            {
                { new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"), new Guid("5efe1909-c0b0-440e-8915-2831570816ec") },
                { new Guid("04b04522-e758-4f8c-b58f-f59dd8de54b5"), new Guid("ead8a2bf-e751-47a9-a2fc-e8b6f643b0c8") },
                { new Guid("073f790e-a105-491d-965c-946e841c3b3e"), new Guid("5efe1909-c0b0-440e-8915-2831570816ec") },
            });
    }
}
