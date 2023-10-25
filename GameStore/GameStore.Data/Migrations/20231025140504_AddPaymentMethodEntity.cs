using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class AddPaymentMethodEntity : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "PaymentMethodId",
            table: "Order",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "PaymentMethod",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Title = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PaymentMethod", x => x.Id);
            });

        migrationBuilder.InsertData(
            table: "Games",
            columns: new[] { "Id", "Alias", "Description", "Discontinued", "Name", "Price", "PublisherId", "UnitInStock" },
            values: new object[,]
            {
                { new Guid("352997f0-9cb6-4951-8b55-10df09d2e168"), "star-wars-jedi", "A 3rd person action-adventure title from Respawn.", false, "Star Wars Jedi: Fallen Order", 1400m, new Guid("08029ea0-8bd8-494c-b3c5-b65a61538f81"), (short)34 },
                { new Guid("4b5f1e22-cd59-4523-a4e9-f0c0239ab820"), "hearthstone", "Hearthstone is a fast-paced strategy card game from Blizzard Entertainment.", false, "Hearthstone", 800m, new Guid("3f8fc430-d0a5-4779-a3a4-5e0add54fde6"), (short)45 },
                { new Guid("e2e928c4-ab49-4bc0-a904-37c34e1385cc"), "overwatch-2", "Overwatch 2 is a free-to-play, team-based action game.", false, "Overwatch 2", 1200m, new Guid("3f8fc430-d0a5-4779-a3a4-5e0add54fde6"), (short)20 },
            });

        migrationBuilder.InsertData(
            table: "PaymentMethod",
            columns: new[] { "Id", "Description", "ImageUrl", "Title" },
            values: new object[,]
            {
                { new Guid("32bda162-d288-4a60-a684-9bd7caf61951"), "Simply pay with IBox!", "https://logowik.com/content/uploads/images/ibox9043.logowik.com.webp", "IBox terminal" },
                { new Guid("77301abc-0738-4540-aa3a-19db9f6bc2dc"), "Use a bank of your choice to make payments!", "https://static.vecteezy.com/system/resources/thumbnails/000/594/232/small/B001.jpg", "Bank" },
                { new Guid("d84def54-1f51-4f1d-aedc-fc1d18b4fa12"), "Pay with your favourite card!", "https://d1yjjnpx0p53s8.cloudfront.net/styles/logo-thumbnail/s3/0013/4323/brand.gif?itok=fSmoZrGH", "Visa" },
            });

        migrationBuilder.UpdateData(
            table: "Platform",
            keyColumn: "Id",
            keyValue: new Guid("467762c6-8a10-4570-b829-e29de78a0757"),
            column: "Type",
            value: "cloud");

        migrationBuilder.InsertData(
            table: "Publisher",
            columns: new[] { "Id", "CompanyName", "Description", "HomePage" },
            values: new object[,]
            {
                { new Guid("defd4ed1-a967-48af-83fb-4e5ffee412b0"), "Nintendo", "Nintendo Co., Ltd.[b] is a Japanese multinational video game company headquartered in Kyoto.", "https://www.nintendo.com/" },
                { new Guid("ec62c5de-e415-4e74-bc75-3a7606563c78"), "Rockstar Games", "Rockstar Games, Inc. is an American video game publisher based in New York City.", "https://www.rockstargames.com/" },
            });

        migrationBuilder.UpdateData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("8e9d1000-50e0-4bd8-8159-42c7431f32b5"),
            column: "PublisherId",
            value: new Guid("defd4ed1-a967-48af-83fb-4e5ffee412b0"));

        migrationBuilder.UpdateData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("95ffb14c-267a-432a-9d7c-22f887290d49"),
            column: "PublisherId",
            value: new Guid("ec62c5de-e415-4e74-bc75-3a7606563c78"));

        migrationBuilder.InsertData(
            table: "GameGenre",
            columns: new[] { "GameId", "GenreId" },
            values: new object[,]
            {
                { new Guid("352997f0-9cb6-4951-8b55-10df09d2e168"), new Guid("dd2bf352-9cfd-4b88-b46f-08217104f90d") },
                { new Guid("352997f0-9cb6-4951-8b55-10df09d2e168"), new Guid("e96511b9-e864-44cc-899f-8313609ffb63") },
                { new Guid("4b5f1e22-cd59-4523-a4e9-f0c0239ab820"), new Guid("073f790e-a105-491d-965c-946e841c3b3e") },
                { new Guid("4b5f1e22-cd59-4523-a4e9-f0c0239ab820"), new Guid("d9eaef23-9680-4d25-869b-4ed91847fd03") },
                { new Guid("e2e928c4-ab49-4bc0-a904-37c34e1385cc"), new Guid("e96511b9-e864-44cc-899f-8313609ffb63") },
                { new Guid("e2e928c4-ab49-4bc0-a904-37c34e1385cc"), new Guid("f82e27c8-760e-4cb8-866f-60bb33caeaac") },
            });

        migrationBuilder.InsertData(
            table: "GamePlatform",
            columns: new[] { "GameId", "PlatformId" },
            values: new object[,]
            {
                { new Guid("352997f0-9cb6-4951-8b55-10df09d2e168"), new Guid("467762c6-8a10-4570-b829-e29de78a0757") },
                { new Guid("352997f0-9cb6-4951-8b55-10df09d2e168"), new Guid("4adb2c38-f819-43cd-aa78-f46d482ceeb7") },
                { new Guid("352997f0-9cb6-4951-8b55-10df09d2e168"), new Guid("83262eb9-517e-4581-b7ba-88b57c33d721") },
                { new Guid("4b5f1e22-cd59-4523-a4e9-f0c0239ab820"), new Guid("467762c6-8a10-4570-b829-e29de78a0757") },
                { new Guid("4b5f1e22-cd59-4523-a4e9-f0c0239ab820"), new Guid("4adb2c38-f819-43cd-aa78-f46d482ceeb7") },
                { new Guid("4b5f1e22-cd59-4523-a4e9-f0c0239ab820"), new Guid("a9f806b4-28c5-4d7b-a776-65dfe029de8f") },
                { new Guid("e2e928c4-ab49-4bc0-a904-37c34e1385cc"), new Guid("4adb2c38-f819-43cd-aa78-f46d482ceeb7") },
                { new Guid("e2e928c4-ab49-4bc0-a904-37c34e1385cc"), new Guid("83262eb9-517e-4581-b7ba-88b57c33d721") },
            });

        migrationBuilder.CreateIndex(
            name: "IX_Order_PaymentMethodId",
            table: "Order",
            column: "PaymentMethodId");

        migrationBuilder.CreateIndex(
            name: "IX_PaymentMethod_Title",
            table: "PaymentMethod",
            column: "Title",
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_Order_PaymentMethod_PaymentMethodId",
            table: "Order",
            column: "PaymentMethodId",
            principalTable: "PaymentMethod",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Order_PaymentMethod_PaymentMethodId",
            table: "Order");

        migrationBuilder.DropTable(
            name: "PaymentMethod");

        migrationBuilder.DropIndex(
            name: "IX_Order_PaymentMethodId",
            table: "Order");

        migrationBuilder.DeleteData(
            table: "GameGenre",
            keyColumns: new[] { "GameId", "GenreId" },
            keyValues: new object[] { new Guid("352997f0-9cb6-4951-8b55-10df09d2e168"), new Guid("dd2bf352-9cfd-4b88-b46f-08217104f90d") });

        migrationBuilder.DeleteData(
            table: "GameGenre",
            keyColumns: new[] { "GameId", "GenreId" },
            keyValues: new object[] { new Guid("352997f0-9cb6-4951-8b55-10df09d2e168"), new Guid("e96511b9-e864-44cc-899f-8313609ffb63") });

        migrationBuilder.DeleteData(
            table: "GameGenre",
            keyColumns: new[] { "GameId", "GenreId" },
            keyValues: new object[] { new Guid("4b5f1e22-cd59-4523-a4e9-f0c0239ab820"), new Guid("073f790e-a105-491d-965c-946e841c3b3e") });

        migrationBuilder.DeleteData(
            table: "GameGenre",
            keyColumns: new[] { "GameId", "GenreId" },
            keyValues: new object[] { new Guid("4b5f1e22-cd59-4523-a4e9-f0c0239ab820"), new Guid("d9eaef23-9680-4d25-869b-4ed91847fd03") });

        migrationBuilder.DeleteData(
            table: "GameGenre",
            keyColumns: new[] { "GameId", "GenreId" },
            keyValues: new object[] { new Guid("e2e928c4-ab49-4bc0-a904-37c34e1385cc"), new Guid("e96511b9-e864-44cc-899f-8313609ffb63") });

        migrationBuilder.DeleteData(
            table: "GameGenre",
            keyColumns: new[] { "GameId", "GenreId" },
            keyValues: new object[] { new Guid("e2e928c4-ab49-4bc0-a904-37c34e1385cc"), new Guid("f82e27c8-760e-4cb8-866f-60bb33caeaac") });

        migrationBuilder.DeleteData(
            table: "GamePlatform",
            keyColumns: new[] { "GameId", "PlatformId" },
            keyValues: new object[] { new Guid("352997f0-9cb6-4951-8b55-10df09d2e168"), new Guid("467762c6-8a10-4570-b829-e29de78a0757") });

        migrationBuilder.DeleteData(
            table: "GamePlatform",
            keyColumns: new[] { "GameId", "PlatformId" },
            keyValues: new object[] { new Guid("352997f0-9cb6-4951-8b55-10df09d2e168"), new Guid("4adb2c38-f819-43cd-aa78-f46d482ceeb7") });

        migrationBuilder.DeleteData(
            table: "GamePlatform",
            keyColumns: new[] { "GameId", "PlatformId" },
            keyValues: new object[] { new Guid("352997f0-9cb6-4951-8b55-10df09d2e168"), new Guid("83262eb9-517e-4581-b7ba-88b57c33d721") });

        migrationBuilder.DeleteData(
            table: "GamePlatform",
            keyColumns: new[] { "GameId", "PlatformId" },
            keyValues: new object[] { new Guid("4b5f1e22-cd59-4523-a4e9-f0c0239ab820"), new Guid("467762c6-8a10-4570-b829-e29de78a0757") });

        migrationBuilder.DeleteData(
            table: "GamePlatform",
            keyColumns: new[] { "GameId", "PlatformId" },
            keyValues: new object[] { new Guid("4b5f1e22-cd59-4523-a4e9-f0c0239ab820"), new Guid("4adb2c38-f819-43cd-aa78-f46d482ceeb7") });

        migrationBuilder.DeleteData(
            table: "GamePlatform",
            keyColumns: new[] { "GameId", "PlatformId" },
            keyValues: new object[] { new Guid("4b5f1e22-cd59-4523-a4e9-f0c0239ab820"), new Guid("a9f806b4-28c5-4d7b-a776-65dfe029de8f") });

        migrationBuilder.DeleteData(
            table: "GamePlatform",
            keyColumns: new[] { "GameId", "PlatformId" },
            keyValues: new object[] { new Guid("e2e928c4-ab49-4bc0-a904-37c34e1385cc"), new Guid("4adb2c38-f819-43cd-aa78-f46d482ceeb7") });

        migrationBuilder.DeleteData(
            table: "GamePlatform",
            keyColumns: new[] { "GameId", "PlatformId" },
            keyValues: new object[] { new Guid("e2e928c4-ab49-4bc0-a904-37c34e1385cc"), new Guid("83262eb9-517e-4581-b7ba-88b57c33d721") });

        migrationBuilder.DeleteData(
            table: "Publisher",
            keyColumn: "Id",
            keyValue: new Guid("defd4ed1-a967-48af-83fb-4e5ffee412b0"));

        migrationBuilder.DeleteData(
            table: "Publisher",
            keyColumn: "Id",
            keyValue: new Guid("ec62c5de-e415-4e74-bc75-3a7606563c78"));

        migrationBuilder.DeleteData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("352997f0-9cb6-4951-8b55-10df09d2e168"));

        migrationBuilder.DeleteData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("4b5f1e22-cd59-4523-a4e9-f0c0239ab820"));

        migrationBuilder.DeleteData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("e2e928c4-ab49-4bc0-a904-37c34e1385cc"));

        migrationBuilder.DropColumn(
            name: "PaymentMethodId",
            table: "Order");

        migrationBuilder.UpdateData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("8e9d1000-50e0-4bd8-8159-42c7431f32b5"),
            column: "PublisherId",
            value: new Guid("3f8fc430-d0a5-4779-a3a4-5e0add54fde6"));

        migrationBuilder.UpdateData(
            table: "Games",
            keyColumn: "Id",
            keyValue: new Guid("95ffb14c-267a-432a-9d7c-22f887290d49"),
            column: "PublisherId",
            value: new Guid("08029ea0-8bd8-494c-b3c5-b65a61538f81"));

        migrationBuilder.UpdateData(
            table: "Platform",
            keyColumn: "Id",
            keyValue: new Guid("467762c6-8a10-4570-b829-e29de78a0757"),
            column: "Type",
            value: "browser");
    }
}
