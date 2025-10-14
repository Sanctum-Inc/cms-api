using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations; 

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Email = table.Column<string>(type: "text", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                Surname = table.Column<string>(type: "text", nullable: false),
                MobileNumber = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CourtCases",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                CaseNumber = table.Column<string>(type: "text", nullable: false),
                Location = table.Column<string>(type: "text", nullable: false),
                Plaintiff = table.Column<string>(type: "text", nullable: false),
                Defendant = table.Column<string>(type: "text", nullable: false),
                Status = table.Column<string>(type: "text", nullable: false),
                Type = table.Column<string>(type: "text", nullable: true),
                Outcome = table.Column<string>(type: "text", nullable: true),
                DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UserId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CourtCases", x => x.Id);
                table.ForeignKey(
                    name: "FK_CourtCases_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Lawyers",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Email = table.Column<string>(type: "text", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                Surname = table.Column<string>(type: "text", nullable: false),
                MobileNumber = table.Column<string>(type: "text", nullable: false),
                UserId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Lawyers", x => x.Id);
                table.ForeignKey(
                    name: "FK_Lawyers_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "CourtCaseDates",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Date = table.Column<string>(type: "text", nullable: false),
                Title = table.Column<string>(type: "text", nullable: false),
                CaseId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CourtCaseDates", x => x.Id);
                table.ForeignKey(
                    name: "FK_CourtCaseDates_CourtCases_CaseId",
                    column: x => x.CaseId,
                    principalTable: "CourtCases",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Documents",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Path = table.Column<string>(type: "text", nullable: false),
                FileName = table.Column<string>(type: "text", nullable: false),
                DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                CaseId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Documents", x => x.Id);
                table.ForeignKey(
                    name: "FK_Documents_CourtCases_CaseId",
                    column: x => x.CaseId,
                    principalTable: "CourtCases",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Documents_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "InvoiceItems",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                InvoiceNumber = table.Column<string>(type: "text", nullable: true),
                Name = table.Column<string>(type: "text", nullable: false),
                Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                Hours = table.Column<int>(type: "integer", nullable: false),
                CostPerHour = table.Column<float>(type: "real", nullable: false),
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                CaseId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_InvoiceItems", x => x.Id);
                table.ForeignKey(
                    name: "FK_InvoiceItems_CourtCases_CaseId",
                    column: x => x.CaseId,
                    principalTable: "CourtCases",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_InvoiceItems_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "CourtCaseLawyer",
            columns: table => new
            {
                CourtCasesId = table.Column<Guid>(type: "uuid", nullable: false),
                LawyersId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CourtCaseLawyer", x => new { x.CourtCasesId, x.LawyersId });
                table.ForeignKey(
                    name: "FK_CourtCaseLawyer_CourtCases_CourtCasesId",
                    column: x => x.CourtCasesId,
                    principalTable: "CourtCases",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_CourtCaseLawyer_Lawyers_LawyersId",
                    column: x => x.LawyersId,
                    principalTable: "Lawyers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "CourtCaseDateLawyer",
            columns: table => new
            {
                CourtCaseDatesId = table.Column<Guid>(type: "uuid", nullable: false),
                LawyersId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CourtCaseDateLawyer", x => new { x.CourtCaseDatesId, x.LawyersId });
                table.ForeignKey(
                    name: "FK_CourtCaseDateLawyer_CourtCaseDates_CourtCaseDatesId",
                    column: x => x.CourtCaseDatesId,
                    principalTable: "CourtCaseDates",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_CourtCaseDateLawyer_Lawyers_LawyersId",
                    column: x => x.LawyersId,
                    principalTable: "Lawyers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_CourtCaseDateLawyer_LawyersId",
            table: "CourtCaseDateLawyer",
            column: "LawyersId");

        migrationBuilder.CreateIndex(
            name: "IX_CourtCaseDates_CaseId",
            table: "CourtCaseDates",
            column: "CaseId");

        migrationBuilder.CreateIndex(
            name: "IX_CourtCaseLawyer_LawyersId",
            table: "CourtCaseLawyer",
            column: "LawyersId");

        migrationBuilder.CreateIndex(
            name: "IX_CourtCases_UserId",
            table: "CourtCases",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_Documents_CaseId",
            table: "Documents",
            column: "CaseId");

        migrationBuilder.CreateIndex(
            name: "IX_Documents_UserId",
            table: "Documents",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_InvoiceItems_CaseId",
            table: "InvoiceItems",
            column: "CaseId");

        migrationBuilder.CreateIndex(
            name: "IX_InvoiceItems_UserId",
            table: "InvoiceItems",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_Lawyers_UserId",
            table: "Lawyers",
            column: "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "CourtCaseDateLawyer");

        migrationBuilder.DropTable(
            name: "CourtCaseLawyer");

        migrationBuilder.DropTable(
            name: "Documents");

        migrationBuilder.DropTable(
            name: "InvoiceItems");

        migrationBuilder.DropTable(
            name: "CourtCaseDates");

        migrationBuilder.DropTable(
            name: "Lawyers");

        migrationBuilder.DropTable(
            name: "CourtCases");

        migrationBuilder.DropTable(
            name: "Users");
    }
}
