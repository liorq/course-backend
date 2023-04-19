using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JwtWebApi.Migrations
{
    public partial class RecreateTablesSVVSwddwddssdvds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "add",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "add",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "add",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "delete",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "add",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "delete",
                table: "Courses");
        }
    }
}
