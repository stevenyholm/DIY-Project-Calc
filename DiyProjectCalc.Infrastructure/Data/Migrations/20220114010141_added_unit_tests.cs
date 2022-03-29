using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiyProjectCalc.Data.Infrastructure.Migrations
{
    public partial class added_unit_tests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DepthNeeded",
                table: "Material",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepthNeeded",
                table: "Material");
        }
    }
}
