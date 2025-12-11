using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDayFeeFromInvoiceItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayFeeAmount",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "IsDayFee",
                table: "InvoiceItems");

            migrationBuilder.AlterColumn<decimal>(
                name: "CostPerHour",
                table: "InvoiceItems",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "CostPerHour",
                table: "InvoiceItems",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<decimal>(
                name: "DayFeeAmount",
                table: "InvoiceItems",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDayFee",
                table: "InvoiceItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
