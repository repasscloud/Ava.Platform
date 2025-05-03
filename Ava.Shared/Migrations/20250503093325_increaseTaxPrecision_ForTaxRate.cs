using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ava.Shared.Migrations
{
    /// <inheritdoc />
    public partial class increaseTaxPrecision_ForTaxRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TaxRate",
                table: "LicenseAgreements",
                type: "numeric(6,4)",
                precision: 6,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,4)",
                oldPrecision: 5,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentOfInvoice",
                table: "LateFeeConfigs",
                type: "numeric(6,4)",
                precision: 6,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,4)",
                oldPrecision: 5,
                oldScale: 4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TaxRate",
                table: "LicenseAgreements",
                type: "numeric(5,4)",
                precision: 5,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(6,4)",
                oldPrecision: 6,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentOfInvoice",
                table: "LateFeeConfigs",
                type: "numeric(5,4)",
                precision: 5,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(6,4)",
                oldPrecision: 6,
                oldScale: 4);
        }
    }
}
