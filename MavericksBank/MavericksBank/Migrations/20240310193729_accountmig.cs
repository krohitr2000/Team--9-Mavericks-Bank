using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MavericksBank.Migrations
{
    public partial class accountmig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IFSCCode",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IFSCCode",
                table: "Account");
        }
    }
}
