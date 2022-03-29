using DiyProjectCalc.Core.Entities.ProjectAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiyProjectCalc.Infrastructure.Data.Config;
public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Project");

        builder.Property(p => p.Name)
        .IsRequired();

        ////TODO: how should this be set up? 
        //var navigationBasicShapes = builder.Metadata.FindNavigation(nameof(Project.BasicShapes));
        //navigationBasicShapes?.SetPropertyAccessMode(PropertyAccessMode.Field);

        //var navigationMaterials = builder.Metadata.FindNavigation(nameof(Project.Materials));
        //navigationMaterials?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
