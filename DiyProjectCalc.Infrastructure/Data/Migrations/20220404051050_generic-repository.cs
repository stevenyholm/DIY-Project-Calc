using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiyProjectCalc.Data.Infrastructure.Migrations
{
    public partial class genericrepository : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicShape_Project_ProjectId",
                table: "BasicShape");

            migrationBuilder.DropForeignKey(
                name: "FK_Material_Project_ProjectId",
                table: "Material");

            migrationBuilder.DropTable(
                name: "MaterialBasicShape");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Material",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "BasicShape",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "BasicShapeMaterial",
                columns: table => new
                {
                    BasicShapesId = table.Column<int>(type: "int", nullable: false),
                    MaterialsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicShapeMaterial", x => new { x.BasicShapesId, x.MaterialsId });
                    table.ForeignKey(
                        name: "FK_BasicShapeMaterial_BasicShape_BasicShapesId",
                        column: x => x.BasicShapesId,
                        principalTable: "BasicShape",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BasicShapeMaterial_Material_MaterialsId",
                        column: x => x.MaterialsId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasicShapeMaterial_MaterialsId",
                table: "BasicShapeMaterial",
                column: "MaterialsId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicShape_Project_ProjectId",
                table: "BasicShape",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Material_Project_ProjectId",
                table: "Material",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicShape_Project_ProjectId",
                table: "BasicShape");

            migrationBuilder.DropForeignKey(
                name: "FK_Material_Project_ProjectId",
                table: "Material");

            migrationBuilder.DropTable(
                name: "BasicShapeMaterial");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Material",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "BasicShape",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "MaterialBasicShape",
                columns: table => new
                {
                    BasicShapeId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialBasicShape", x => new { x.BasicShapeId, x.MaterialId });
                    table.ForeignKey(
                        name: "FK_MaterialBasicShape_BasicShape_BasicShapeId",
                        column: x => x.BasicShapeId,
                        principalTable: "BasicShape",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialBasicShape_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialBasicShape_MaterialId",
                table: "MaterialBasicShape",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicShape_Project_ProjectId",
                table: "BasicShape",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Material_Project_ProjectId",
                table: "Material",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
