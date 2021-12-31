using DiyProjectCalc.Models;
using Microsoft.EntityFrameworkCore;

namespace DiyProjectCalc.Data;

public class ApplicationDbContext : DbContext //TODO: add sealed keyword
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Material>().ToTable("Material"); //TODO: change other table names to be singular

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
