using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ava.Shared.Migrations
{
    /// <inheritdoc />
    public partial class updateAvaClientTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CliendID",
                table: "AvaClients",
                newName: "ClientID");

            migrationBuilder.AddColumn<string>(
                name: "AdminPersonCountryCode",
                table: "AvaClients",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingPersonCountryCode",
                table: "AvaClients",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactPersonCountryCode",
                table: "AvaClients",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LicenseAgreementId",
                table: "AvaClients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxID",
                table: "AvaClients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxIDType",
                table: "AvaClients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TaxLastValidated",
                table: "AvaClients",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminPersonCountryCode",
                table: "AvaClients");

            migrationBuilder.DropColumn(
                name: "BillingPersonCountryCode",
                table: "AvaClients");

            migrationBuilder.DropColumn(
                name: "ContactPersonCountryCode",
                table: "AvaClients");

            migrationBuilder.DropColumn(
                name: "LicenseAgreementId",
                table: "AvaClients");

            migrationBuilder.DropColumn(
                name: "TaxID",
                table: "AvaClients");

            migrationBuilder.DropColumn(
                name: "TaxIDType",
                table: "AvaClients");

            migrationBuilder.DropColumn(
                name: "TaxLastValidated",
                table: "AvaClients");

            migrationBuilder.RenameColumn(
                name: "ClientID",
                table: "AvaClients",
                newName: "CliendID");
        }
    }
}
