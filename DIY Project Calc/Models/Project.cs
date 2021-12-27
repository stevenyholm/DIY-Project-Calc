using System.ComponentModel.DataAnnotations;

namespace DIY_Project_Calc.Models;

public class Project
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Display(Name = "Project Name")]
    public string Name { get; set; }
}
