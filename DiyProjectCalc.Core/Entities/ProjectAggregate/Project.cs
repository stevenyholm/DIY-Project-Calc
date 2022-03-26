
//using DiyProjectCalc.SharedKernel;
//using DiyProjectCalc.SharedKernel.Interfaces;

//namespace DiyProjectCalc.Core.Entities.ProjectAggregate;

//public class Project : BaseEntity, IAggregateRoot
//{
//    //TODO: [Key]
//    //public int ProjectId { get; set; }

//    //TODO: [Display(Name = "Project Name")]
//    public string? Name { get; set; }

//    public virtual ICollection<BasicShape> BasicShapes { get; set; } = new HashSet<BasicShape>();
//    public virtual ICollection<Material> Materials { get; set; } = new HashSet<Material>();

//}

using System.ComponentModel.DataAnnotations;

namespace DiyProjectCalc.Core.Entities.ProjectAggregate;

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