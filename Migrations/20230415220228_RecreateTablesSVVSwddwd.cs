using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JwtWebApi.Migrations
{
    public partial class RecreateTablesSVVSwddwd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "hours",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "hours",
                table: "Courses");
        }
    }
}
