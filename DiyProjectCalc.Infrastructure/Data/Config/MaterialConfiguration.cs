using DiyProjectCalc.Core.Entities.ProjectAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiyProjectCalc.Infrastructure.Data.Config;
public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.ToTable("Material");

        //fixes cascade path error and naming conventions that code generator would add
        builder
            .HasMany(p => p.BasicShapes)
            .WithMany(p => p.Materials)
            .UsingEntity<Dictionary<string, object>>(
                "MaterialBasicShape",
                j => j
                    .HasOne<BasicShape>()
                    .WithMany()
                    .HasForeignKey("BasicShapeId")
                    .HasConstraintName("FK_MaterialBasicShape_BasicShape_BasicShapeId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Material>()
                    .WithMany()
                    .HasForeignKey("MaterialId")
                    .HasConstraintName("FK_MaterialBasicShape_Material_MaterialId")
                    .OnDelete(DeleteBehavior.NoAction));

        //var navigationBasicShapes = builder.Metadata.FindNavigation(nameof(Project.BasicShapes));
        //navigationBasicShapes?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}