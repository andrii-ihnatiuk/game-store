using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class AddIdentity : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Comment_Games_GameId",
            table: "Comment");

        migrationBuilder.DropForeignKey(
            name: "FK_GameGenre_Games_GameId",
            table: "GameGenre");

        migrationBuilder.DropForeignKey(
            name: "FK_GameGenre_Genres_GenreId",
            table: "GameGenre");

        migrationBuilder.DropForeignKey(
            name: "FK_GamePlatform_Games_GameId",
            table: "GamePlatform");

        migrationBuilder.DropForeignKey(
            name: "FK_Games_Publisher_PublisherId",
            table: "Games");

        migrationBuilder.DropForeignKey(
            name: "FK_Genres_Genres_ParentGenreId",
            table: "Genres");

        migrationBuilder.DropForeignKey(
            name: "FK_OrderDetail_Games_ProductId",
            table: "OrderDetail");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Genres",
            table: "Genres");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Games",
            table: "Games");

        migrationBuilder.RenameTable(
            name: "Genres",
            newName: "Genre");

        migrationBuilder.RenameTable(
            name: "Games",
            newName: "Game");

        migrationBuilder.RenameIndex(
            name: "IX_Genres_ParentGenreId",
            table: "Genre",
            newName: "IX_Genre_ParentGenreId");

        migrationBuilder.RenameIndex(
            name: "IX_Genres_Name",
            table: "Genre",
            newName: "IX_Genre_Name");

        migrationBuilder.RenameIndex(
            name: "IX_Games_PublisherId",
            table: "Game",
            newName: "IX_Game_PublisherId");

        migrationBuilder.RenameIndex(
            name: "IX_Games_Alias",
            table: "Game",
            newName: "IX_Game_Alias");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Genre",
            table: "Genre",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Game",
            table: "Game",
            column: "Id");

        migrationBuilder.CreateTable(
            name: "AspNetRoles",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUsers",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                AccessFailedCount = table.Column<int>(type: "int", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUsers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AspNetRoleClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AspNetRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserLogins",
            columns: table => new
            {
                LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                table.ForeignKey(
                    name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserRoles",
            columns: table => new
            {
                UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                table.ForeignKey(
                    name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AspNetRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserTokens",
            columns: table => new
            {
                UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                table.ForeignKey(
                    name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_AspNetRoleClaims_RoleId",
            table: "AspNetRoleClaims",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "RoleNameIndex",
            table: "AspNetRoles",
            column: "NormalizedName",
            unique: true,
            filter: "[NormalizedName] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserClaims_UserId",
            table: "AspNetUserClaims",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserLogins_UserId",
            table: "AspNetUserLogins",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserRoles_RoleId",
            table: "AspNetUserRoles",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "EmailIndex",
            table: "AspNetUsers",
            column: "NormalizedEmail");

        migrationBuilder.CreateIndex(
            name: "UserNameIndex",
            table: "AspNetUsers",
            column: "NormalizedUserName",
            unique: true,
            filter: "[NormalizedUserName] IS NOT NULL");

        migrationBuilder.AddForeignKey(
            name: "FK_Comment_Game_GameId",
            table: "Comment",
            column: "GameId",
            principalTable: "Game",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Game_Publisher_PublisherId",
            table: "Game",
            column: "PublisherId",
            principalTable: "Publisher",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);

        migrationBuilder.AddForeignKey(
            name: "FK_GameGenre_Game_GameId",
            table: "GameGenre",
            column: "GameId",
            principalTable: "Game",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_GameGenre_Genre_GenreId",
            table: "GameGenre",
            column: "GenreId",
            principalTable: "Genre",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_GamePlatform_Game_GameId",
            table: "GamePlatform",
            column: "GameId",
            principalTable: "Game",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Genre_Genre_ParentGenreId",
            table: "Genre",
            column: "ParentGenreId",
            principalTable: "Genre",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_OrderDetail_Game_ProductId",
            table: "OrderDetail",
            column: "ProductId",
            principalTable: "Game",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Comment_Game_GameId",
            table: "Comment");

        migrationBuilder.DropForeignKey(
            name: "FK_Game_Publisher_PublisherId",
            table: "Game");

        migrationBuilder.DropForeignKey(
            name: "FK_GameGenre_Game_GameId",
            table: "GameGenre");

        migrationBuilder.DropForeignKey(
            name: "FK_GameGenre_Genre_GenreId",
            table: "GameGenre");

        migrationBuilder.DropForeignKey(
            name: "FK_GamePlatform_Game_GameId",
            table: "GamePlatform");

        migrationBuilder.DropForeignKey(
            name: "FK_Genre_Genre_ParentGenreId",
            table: "Genre");

        migrationBuilder.DropForeignKey(
            name: "FK_OrderDetail_Game_ProductId",
            table: "OrderDetail");

        migrationBuilder.DropTable(
            name: "AspNetRoleClaims");

        migrationBuilder.DropTable(
            name: "AspNetUserClaims");

        migrationBuilder.DropTable(
            name: "AspNetUserLogins");

        migrationBuilder.DropTable(
            name: "AspNetUserRoles");

        migrationBuilder.DropTable(
            name: "AspNetUserTokens");

        migrationBuilder.DropTable(
            name: "AspNetRoles");

        migrationBuilder.DropTable(
            name: "AspNetUsers");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Genre",
            table: "Genre");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Game",
            table: "Game");

        migrationBuilder.RenameTable(
            name: "Genre",
            newName: "Genres");

        migrationBuilder.RenameTable(
            name: "Game",
            newName: "Games");

        migrationBuilder.RenameIndex(
            name: "IX_Genre_ParentGenreId",
            table: "Genres",
            newName: "IX_Genres_ParentGenreId");

        migrationBuilder.RenameIndex(
            name: "IX_Genre_Name",
            table: "Genres",
            newName: "IX_Genres_Name");

        migrationBuilder.RenameIndex(
            name: "IX_Game_PublisherId",
            table: "Games",
            newName: "IX_Games_PublisherId");

        migrationBuilder.RenameIndex(
            name: "IX_Game_Alias",
            table: "Games",
            newName: "IX_Games_Alias");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Genres",
            table: "Genres",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Games",
            table: "Games",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Comment_Games_GameId",
            table: "Comment",
            column: "GameId",
            principalTable: "Games",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_GameGenre_Games_GameId",
            table: "GameGenre",
            column: "GameId",
            principalTable: "Games",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_GameGenre_Genres_GenreId",
            table: "GameGenre",
            column: "GenreId",
            principalTable: "Genres",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_GamePlatform_Games_GameId",
            table: "GamePlatform",
            column: "GameId",
            principalTable: "Games",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Games_Publisher_PublisherId",
            table: "Games",
            column: "PublisherId",
            principalTable: "Publisher",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);

        migrationBuilder.AddForeignKey(
            name: "FK_Genres_Genres_ParentGenreId",
            table: "Genres",
            column: "ParentGenreId",
            principalTable: "Genres",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_OrderDetail_Games_ProductId",
            table: "OrderDetail",
            column: "ProductId",
            principalTable: "Games",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
