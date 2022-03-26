using DiyProjectCalc.Core.Entities.ProjectAggregate;
using Microsoft.EntityFrameworkCore;

namespace DiyProjectCalc.Data;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Make SQL table names be singular, while C# DbSet names are plural
        modelBuilder.Entity<Project>().ToTable("Project");
        modelBuilder.Entity<Material>().ToTable("Material");
        modelBuilder.Entity<BasicShape>().ToTable("BasicShape"); 

        //fixes cascade path error and naming conventions that code generator would add
        modelBuilder.Entity<Material>()
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
    }

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Material> Materials => Set<Material>();
    public DbSet<BasicShape> BasicShapes => Set<BasicShape>();
}
