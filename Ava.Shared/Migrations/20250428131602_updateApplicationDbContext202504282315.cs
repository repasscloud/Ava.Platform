using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ava.Shared.Migrations
{
    /// <inheritdoc />
    public partial class updateApplicationDbContext202504282315 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LateFeeConfigs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    GracePeriodDays = table.Column<int>(type: "integer", nullable: false),
                    UseFixedAmount = table.Column<bool>(type: "boolean", nullable: false),
                    FixedAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    PercentOfInvoice = table.Column<decimal>(type: "numeric", nullable: false),
                    RecurringOption = table.Column<int>(type: "integer", nullable: false),
                    MaxLateFeeCap = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LateFeeConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LicenseAgreements",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    AvaClientId = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    PaymentTerms = table.Column<int>(type: "integer", nullable: false),
                    PaymentMethod = table.Column<int>(type: "integer", nullable: false),
                    BillingType = table.Column<int>(type: "integer", nullable: false),
                    BillingFrequency = table.Column<int>(type: "integer", nullable: false),
                    RemittanceEmail = table.Column<string>(type: "text", nullable: false),
                    AutoRenew = table.Column<bool>(type: "boolean", nullable: false),
                    GracePeriodDays = table.Column<int>(type: "integer", nullable: false),
                    AccessFee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PrepaidBalance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentStatus = table.Column<int>(type: "integer", nullable: false),
                    LateFeeConfigId = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    AccountThreshold = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TaxRate = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    TrialEndsOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Discount = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    DiscountExpires = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PnrCreationFee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PnrChangeFee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    FlightMarkupPercent = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    FlightPerItemFee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    FlightFeeType = table.Column<int>(type: "integer", nullable: false),
                    HotelMarkupPercent = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    HotelPerItemFee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    HotelFeeType = table.Column<int>(type: "integer", nullable: false),
                    CarMarkupPercent = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    CarPerItemFee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CarFeeType = table.Column<int>(type: "integer", nullable: false),
                    RailMarkupPercent = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    RailPerItemFee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    RailFeeType = table.Column<int>(type: "integer", nullable: false),
                    TransferMarkupPercent = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    TransferPerItemFee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TransferFeeType = table.Column<int>(type: "integer", nullable: false),
                    ActivityMarkupPercent = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    ActivityPerItemFee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ActivityFeeType = table.Column<int>(type: "integer", nullable: false),
                    TravelMarkupPercent = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    TravelPerItemFee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TravelFeeType = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseAgreements", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LateFeeConfigs");

            migrationBuilder.DropTable(
                name: "LicenseAgreements");
        }
    }
}
