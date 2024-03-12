using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MavericksBank.Migrations
{
    public partial class branchmig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BranchDetails",
                columns: table => new
                {
                    IFSCCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchDetails", x => x.IFSCCode);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchDetails");
        }
    }
}
