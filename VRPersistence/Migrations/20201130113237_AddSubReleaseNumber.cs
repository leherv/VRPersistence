using Microsoft.EntityFrameworkCore.Migrations;

namespace VRPersistence.Migrations
{
    public partial class AddSubReleaseNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubReleaseNumber",
                table: "Releases",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubReleaseNumber",
                table: "Releases");
        }
    }
}
