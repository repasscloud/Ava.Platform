using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ava.Shared.Migrations
{
    /// <inheritdoc />
    public partial class updateAvavEmployeesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AvaEmployees",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AvaEmployees",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "AvaEmployees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "AvaEmployees",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VerificationToken",
                table: "AvaEmployees",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AvaEmployees");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "AvaEmployees");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "AvaEmployees");

            migrationBuilder.DropColumn(
                name: "VerificationToken",
                table: "AvaEmployees");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "AvaEmployees",
                newName: "Name");
        }
    }
}
