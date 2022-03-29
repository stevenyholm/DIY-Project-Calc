using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiyProjectCalc.Data.Infrastructure.Migrations
{
    public partial class rename_pk_fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Project",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MaterialId",
                table: "Material",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "BasicShapeId",
                table: "BasicShape",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Project",
                newName: "ProjectId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Material",
                newName: "MaterialId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "BasicShape",
                newName: "BasicShapeId");
        }
    }
}
