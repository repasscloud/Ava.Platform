using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ava.Shared.Migrations
{
    /// <inheritdoc />
    public partial class addTravelRecordSearchTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupportedEmailDomains",
                table: "AvaClientLicenses");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "AvaClientLicenses",
                newName: "ClientID");

            migrationBuilder.RenameColumn(
                name: "AppId",
                table: "AvaClientLicenses",
                newName: "AppID");

            migrationBuilder.AlterColumn<string>(
                name: "LicenseId",
                table: "AvaSalesRecords",
                type: "character varying(22)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SupportedEmailDomain",
                table: "AvaClientSupportedDomains",
                type: "character varying(253)",
                maxLength: 253,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)");

            migrationBuilder.AddColumn<string>(
                name: "DefaultCurrency",
                table: "AvaClients",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "SpendThreshold",
                table: "AvaClientLicenses",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Signature",
                table: "AvaClientLicenses",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ClientID",
                table: "AvaClientLicenses",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)");

            migrationBuilder.AlterColumn<string>(
                name: "AppID",
                table: "AvaClientLicenses",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AvaClientLicenses",
                type: "character varying(22)",
                maxLength: 22,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "TravelSearchRecords",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SearchId = table.Column<string>(type: "text", nullable: false),
                    TravelType = table.Column<int>(type: "integer", nullable: false),
                    FlightSubComponent = table.Column<int>(type: "integer", nullable: false),
                    HotelSubComponent = table.Column<int>(type: "integer", nullable: false),
                    CarSubComponent = table.Column<int>(type: "integer", nullable: false),
                    RailSubComponent = table.Column<int>(type: "integer", nullable: false),
                    TransferSubComponent = table.Column<int>(type: "integer", nullable: false),
                    ActivitySubComponent = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Payload = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelSearchRecords", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TravelSearchRecords");

            migrationBuilder.DropColumn(
                name: "DefaultCurrency",
                table: "AvaClients");

            migrationBuilder.RenameColumn(
                name: "ClientID",
                table: "AvaClientLicenses",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "AppID",
                table: "AvaClientLicenses",
                newName: "AppId");

            migrationBuilder.AlterColumn<string>(
                name: "LicenseId",
                table: "AvaSalesRecords",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(22)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SupportedEmailDomain",
                table: "AvaClientSupportedDomains",
                type: "varchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(253)",
                oldMaxLength: 253);

            migrationBuilder.AlterColumn<int>(
                name: "SpendThreshold",
                table: "AvaClientLicenses",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Signature",
                table: "AvaClientLicenses",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "AvaClientLicenses",
                type: "varchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "AppId",
                table: "AvaClientLicenses",
                type: "varchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AvaClientLicenses",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(22)",
                oldMaxLength: 22);

            migrationBuilder.AddColumn<string>(
                name: "SupportedEmailDomains",
                table: "AvaClientLicenses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
