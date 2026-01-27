using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class LinkUserToFirm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FirmId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FirmId",
                table: "Users",
                column: "FirmId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Firms_FirmId",
                table: "Users",
                column: "FirmId",
                principalTable: "Firms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Firms_FirmId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_FirmId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FirmId",
                table: "Users");
        }
    }
}
