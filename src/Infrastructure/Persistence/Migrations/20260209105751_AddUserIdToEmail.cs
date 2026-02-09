using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lawyers_Users_CreatedByUserId",
                table: "Lawyers");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "Lawyers",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Lawyers_CreatedByUserId",
                table: "Lawyers",
                newName: "IX_Lawyers_UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Email",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Lawyers_Users_UserId",
                table: "Lawyers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lawyers_Users_UserId",
                table: "Lawyers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Email");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Lawyers",
                newName: "CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Lawyers_UserId",
                table: "Lawyers",
                newName: "IX_Lawyers_CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lawyers_Users_CreatedByUserId",
                table: "Lawyers",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
