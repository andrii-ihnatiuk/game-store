using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.Data.Migrations;

/// <inheritdoc />
public partial class UpdateComment : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Intro",
            table: "Comment",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<string>(
            name: "Type",
            table: "Comment",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Intro",
            table: "Comment");

        migrationBuilder.DropColumn(
            name: "Type",
            table: "Comment");
    }
}
