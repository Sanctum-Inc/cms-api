using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class AddDocumentAttributes : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "Path",
            table: "Documents",
            newName: "Name");

        migrationBuilder.AddColumn<string>(
            name: "ContentType",
            table: "Documents",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<long>(
            name: "Size",
            table: "Documents",
            type: "bigint",
            nullable: false,
            defaultValue: 0L);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "ContentType",
            table: "Documents");

        migrationBuilder.DropColumn(
            name: "Size",
            table: "Documents");

        migrationBuilder.RenameColumn(
            name: "Name",
            table: "Documents",
            newName: "Path");
    }
}
