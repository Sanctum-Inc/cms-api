using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditableEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Users",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "InvoiceItems",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Documents",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "CourtCases",
                newName: "Created");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Lawyers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Lawyers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Lawyers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Lawyers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Lawyers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "InvoiceItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "InvoiceItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "InvoiceItems",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "InvoiceItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Documents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Documents",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Documents",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Documents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CourtCases",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CourtCases",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "CourtCases",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "CourtCases",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "CourtCaseDates",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CourtCaseDates",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CourtCaseDates",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "CourtCaseDates",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "CourtCaseDates",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Lawyers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Lawyers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Lawyers");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Lawyers");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Lawyers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CourtCases");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CourtCases");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "CourtCases");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "CourtCases");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "CourtCaseDates");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CourtCaseDates");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CourtCaseDates");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "CourtCaseDates");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "CourtCaseDates");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Users",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "InvoiceItems",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Documents",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "CourtCases",
                newName: "DateCreated");
        }
    }
}
