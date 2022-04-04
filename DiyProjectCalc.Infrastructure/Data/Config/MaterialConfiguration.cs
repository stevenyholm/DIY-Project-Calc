using DiyProjectCalc.Core.Entities.ProjectAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiyProjectCalc.Infrastructure.Data.Config;
public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.ToTable("Material");

        //var navigationBasicShapes = builder.Metadata.FindNavigation(nameof(Project.BasicShapes));
        //navigationBasicShapes?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}