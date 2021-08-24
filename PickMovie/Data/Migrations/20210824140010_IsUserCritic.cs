using Microsoft.EntityFrameworkCore.Migrations;

namespace PickMovie.Data.Migrations
{
    public partial class IsUserCritic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCritic",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCritic",
                table: "AspNetUsers");
        }
    }
}
