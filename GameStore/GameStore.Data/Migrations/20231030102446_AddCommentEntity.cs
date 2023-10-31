using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class AddCommentEntity : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Comment",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Comment", x => x.Id);
                table.ForeignKey(
                    name: "FK_Comment_Comment_ParentId",
                    column: x => x.ParentId,
                    principalTable: "Comment",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Comment_Games_GameId",
                    column: x => x.GameId,
                    principalTable: "Games",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Comment_GameId",
            table: "Comment",
            column: "GameId");

        migrationBuilder.CreateIndex(
            name: "IX_Comment_ParentId",
            table: "Comment",
            column: "ParentId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Comment");
    }
}
