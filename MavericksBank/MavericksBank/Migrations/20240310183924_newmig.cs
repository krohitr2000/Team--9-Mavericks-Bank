using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MavericksBank.Migrations
{
    public partial class newmig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankBranch");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankBranch",
                columns: table => new
                {
                    IFSCCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankBranch", x => x.IFSCCode);
                });
        }
    }
}
