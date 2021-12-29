using System.ComponentModel.DataAnnotations;

namespace DIY_Project_Calc.Models;

public class Project 
{
    [Key]
    public int ProjectId { get; set; }

    [Required]
    [Display(Name = "Project Name")]
    public string? Name { get; set; }

    public virtual ICollection<BasicShape> BasicShapes { get; set; } = new HashSet<BasicShape>();

    public int Area() => BasicShapes.Select(a => a.Area()).Sum();
}
