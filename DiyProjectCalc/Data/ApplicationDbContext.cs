using DIY_Project_Calc.Models;
using Microsoft.EntityFrameworkCore;

namespace DIY_Project_Calc.Data;

public class ApplicationDbContext : DbContext 
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<BasicShape> BasicShapes => Set<BasicShape>();
}
