using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dashboard.Migrations
{
    public partial class m_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "teamData",
                columns: table => new
                {
                    dataId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    wins = table.Column<int>(nullable: false),
                    loses = table.Column<int>(nullable: false),
                    draws = table.Column<int>(nullable: false),
                    year = table.Column<string>(nullable: true),
                    teamId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teamData", x => x.dataId);
                    table.ForeignKey(
                        name: "FK_teamData_team_teamId",
                        column: x => x.teamId,
                        principalTable: "team",
                        principalColumn: "teamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_teamData_teamId",
                table: "teamData",
                column: "teamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "teamData");
        }
    }
}
