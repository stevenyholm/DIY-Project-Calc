using DiyProjectCalc.Core.Entities.ProjectAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiyProjectCalc.Infrastructure.Data.Config;
public class BasicShapeConfiguration : IEntityTypeConfiguration<BasicShape>
{
    public void Configure(EntityTypeBuilder<BasicShape> builder)
    {
        builder.ToTable("BasicShape");

        //var navigationMaterials = builder.Metadata.FindNavigation(nameof(Project.Materials));
        //navigationMaterials?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}