using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiyProjectCalc.Migrations
{
    public partial class table_names_singular : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicShapes_Projects_ProjectId",
                table: "BasicShapes");

            migrationBuilder.DropForeignKey(
                name: "FK_Material_Projects_ProjectId",
                table: "Material");

            migrationBuilder.DropForeignKey(
                name: "FK_MaterialBasicShape_BasicShape_BasicShapeId",
                table: "MaterialBasicShape");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BasicShapes",
                table: "BasicShapes");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "Project");

            migrationBuilder.RenameTable(
                name: "BasicShapes",
                newName: "BasicShape");

            migrationBuilder.RenameIndex(
                name: "IX_BasicShapes_ProjectId",
                table: "BasicShape",
                newName: "IX_BasicShape_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Project",
                table: "Project",
                column: "ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BasicShape",
                table: "BasicShape",
                column: "BasicShapeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicShape_Project_ProjectId",
                table: "BasicShape",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Material_Project_ProjectId",
                table: "Material",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialBasicShape_BasicShape_BasicShapeId",
                table: "MaterialBasicShape",
                column: "BasicShapeId",
                principalTable: "BasicShape",
                principalColumn: "BasicShapeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicShape_Project_ProjectId",
                table: "BasicShape");

            migrationBuilder.DropForeignKey(
                name: "FK_Material_Project_ProjectId",
                table: "Material");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Project",
                table: "Project");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BasicShape",
                table: "BasicShape");

            migrationBuilder.RenameTable(
                name: "Project",
                newName: "Projects");

            migrationBuilder.RenameTable(
                name: "BasicShape",
                newName: "BasicShapes");

            migrationBuilder.RenameIndex(
                name: "IX_BasicShape_ProjectId",
                table: "BasicShapes",
                newName: "IX_BasicShapes_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects",
                table: "Projects",
                column: "ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BasicShapes",
                table: "BasicShapes",
                column: "BasicShapeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicShapes_Projects_ProjectId",
                table: "BasicShapes",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Material_Projects_ProjectId",
                table: "Material",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
