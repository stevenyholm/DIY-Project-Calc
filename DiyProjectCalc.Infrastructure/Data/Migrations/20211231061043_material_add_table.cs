using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiyProjectCalc.Data.Infrastructure.Migrations
{
    public partial class material_add_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Material",
                columns: table => new
                {
                    MaterialId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasurementType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Length = table.Column<double>(type: "float", nullable: true),
                    Width = table.Column<double>(type: "float", nullable: true),
                    Depth = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Material", x => x.MaterialId);
                    table.ForeignKey(
                        name: "FK_Material_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

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
                        principalTable: "BasicShapes",
                        principalColumn: "BasicShapeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialBasicShape_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "MaterialId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Material_ProjectId",
                table: "Material",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialBasicShape_MaterialId",
                table: "MaterialBasicShape",
                column: "MaterialId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialBasicShape");

            migrationBuilder.DropTable(
                name: "Material");
        }
    }
}
