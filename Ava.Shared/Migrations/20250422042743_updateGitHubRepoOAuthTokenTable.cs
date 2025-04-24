using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ava.Shared.Migrations
{
    /// <inheritdoc />
    public partial class updateGitHubRepoOAuthTokenTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GitHubRepoOAuthTokens",
                table: "GitHubRepoOAuthTokens");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "GitHubRepoOAuthTokens",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GitHubRepoOAuthTokens",
                table: "GitHubRepoOAuthTokens",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GitHubRepoOAuthTokens",
                table: "GitHubRepoOAuthTokens");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GitHubRepoOAuthTokens");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GitHubRepoOAuthTokens",
                table: "GitHubRepoOAuthTokens",
                column: "Token");
        }
    }
}
