using System.ComponentModel.DataAnnotations;

namespace DiyProjectCalc.Models;

public class Project 
{
    [Key]
    public int ProjectId { get; set; }

    [Required]
    [Display(Name = "Project Name")]
    public string? Name { get; set; }

    public virtual ICollection<BasicShape> BasicShapes { get; set; } = new HashSet<BasicShape>();
    public virtual ICollection<Material> Materials { get; set; } = new HashSet<Material>();

}
