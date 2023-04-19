using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JwtWebApi.Migrations
{
    public partial class MyFirstMigrationdcd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoursesName",
                table: "ClassAttendees",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoursesName",
                table: "ClassAttendees");
        }
    }
}
