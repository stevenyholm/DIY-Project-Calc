using DiyProjectCalc.Core.Entities.ProjectAggregate;
using DiyProjectCalc.Infrastructure.Data;
using DiyProjectCalc.Models.DTO;
using DiyProjectCalc.TestHelpers.TestData.Abstractions;
using DiyProjectCalc.TestHelpers.TestModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DiyProjectCalc.TestHelpers.TestData;

public class ProjectTestData
{
    public static readonly string ValidName = "Patio";
    public static readonly int ValidProjectCountBasicShapes = 6;
    public static readonly int ValidProjectCountMaterials = 3;
    public static readonly int ValidProjectListCount = 3;

    public static Project NewProject
    {
        get => new Project()
        {
            Name = "New Project"
        };
    }

    public static ProjectDTO NewProjectDTO
    {
        get => new ProjectDTO(
            Name: ProjectTestData.NewProject.Name
            );
    }

    public static Project MockSimpleProject
    {
        get => new Project()
        {
            Id = 99,
            Name = "New Project"
        };
    }
    public static ProjectDTO MockSimpleProjectDTO
    {
        get => new ProjectDTO(
            Id: ProjectTestData.MockSimpleProject.Id,
            Name: ProjectTestData.MockSimpleProject.Name
            );
    }

    public static Project MockSimpleProjectWithBasicShapes
    {
        get => new Project()
        {
            Id = ProjectTestData.MockSimpleProject.Id,
            Name = ProjectTestData.MockSimpleProject.Name,
            BasicShapes = MockBasicShapesCollection
        };
    }

    public static Project MockSimpleProjectWithMaterials
    {
        get => new Project()
        {
            Id = ProjectTestData.MockSimpleProject.Id,
            Name = ProjectTestData.MockSimpleProject.Name,
            Materials = MockMaterialsCollection
        };
    }

    private static ICollection<BasicShape> MockBasicShapesCollection
    {
        get
        {
            var collection = new List<BasicShape>();
            collection.Add(BasicShapeTestData.MockSimpleBasicShape);
            collection.AddRange(BasicShapeTestData.BasicShapesFor(new BasicShapeTestData().TestDataForPatioArea));
            return collection;
        }
    }

    private static ICollection<Material> MockMaterialsCollection
    {
        get
        {
            var collection = new List<Material>(); 
            collection.Add(MaterialTestData.MockSimpleMaterial);
            collection.AddRange(MaterialTestData.MaterialsFor(new MaterialTestData().TestDataForPatio));
            return collection;
        }
    }

    public static int MockSimpleProjectId
    {
        get => MockSimpleProject.Id;
    }

    public static int MockSimpleProjectCountBasicShapes
    {
        get => MockSimpleProject.BasicShapes.Count;
    }

    public static int MockSimpleProjectCountMaterials
    {
        get => MockSimpleProject.Materials.Count;
    }

    public ProjectTestData()
    {
        var basicShapesTestData = new BasicShapeTestData();
        var materialsTestData = new MaterialTestData(basicShapesTestData);

        InitTestModels(materialsTestData, basicShapesTestData);
    }

    public static int ValidProjectId(ApplicationDbContext dbContext) =>
        dbContext.Projects.AsNoTracking().FirstOrDefault(p => p.Name == ProjectTestData.ValidName)?.Id ?? 0;

    public static Project? ValidProject(ApplicationDbContext dbContext) =>
        dbContext.Projects.AsNoTracking()
        .Include(project => project.BasicShapes)
        .Include(project => project.Materials)
        .FirstOrDefault(p => p.Name == ProjectTestData.ValidName);

    public static Project? ValidProject(ApplicationDbContext dbContext, int projectId) =>
        dbContext.Projects.AsNoTracking().FirstOrDefault(p => p.Id == projectId);

    public static int ProjectsCount(ApplicationDbContext dbContext) =>
        dbContext.Projects.AsNoTracking().Count();
    public static int ProjectMaterialsCount(ApplicationDbContext dbContext, int projectId) 
    {
        var projects = dbContext.Projects.AsNoTracking().Include(project => project.Materials)
            .FirstOrDefault(p => p.Id == projectId);
        return projects!.Materials.Count();
    }
    public static int ProjectBasicShapesCount(ApplicationDbContext dbContext, int projectId)
    {
        var projects = dbContext.Projects.AsNoTracking().Include(project => project.BasicShapes)
            .FirstOrDefault(p => p.Id == projectId);
        return projects!.BasicShapes.Count();
    }
    public static ProjectDTO? ValidProjectDTO(ApplicationDbContext dbContext) =>
        new ProjectDTO(
            Id: ProjectTestData.ValidProject(dbContext)?.Id ?? -1,
            Name: ProjectTestData.ValidProject(dbContext)?.Name
            );



    //==============================================================================================
    //===================================================================    TestModel    ==========
    //==============================================================================================


    public ProjectTestModel? PatioProjectTestData;  //use this to test "happy path"
    public ProjectTestModel? FamilyRoomFlooringProjectTestData;
    public ProjectTestModel? ReplaceBaseboardsProjectTestData;

    private void InitTestModels(MaterialTestData materialsTestData, BasicShapeTestData basicShapesTestData)
    {
        PatioProjectTestData = new ProjectTestModel()
        {
            TestCaseName = "Patio measures in volume",
            Project = new Project()
            {
                Materials = MaterialTestData.MaterialsFor(materialsTestData.TestDataForPatio),
                BasicShapes = BasicShapeTestData.BasicShapesFor(basicShapesTestData.TestDataForPatioCombined),
                Name = ValidName
            }
        };

        FamilyRoomFlooringProjectTestData = new ProjectTestModel()
        {
            TestCaseName = "Family Room Flooring measures in area",
            Project = new Project()
            {
                Materials = MaterialTestData.MaterialsFor(materialsTestData.TestDataForFamilyRoomFlooring),
                BasicShapes = BasicShapeTestData.BasicShapesFor(basicShapesTestData.TestDataForFamilyRoomFlooringArea),
                Name = "Family Room Flooring"
            }
        };

        ReplaceBaseboardsProjectTestData = new ProjectTestModel()
        {
            TestCaseName = "Replace Baseboards measures in linear",
            Project = new Project()
            {
                Materials = MaterialTestData.MaterialsFor(materialsTestData.TestDataForReplaceBaseboards),
                BasicShapes = BasicShapeTestData.BasicShapesFor(basicShapesTestData.TestDataForReplaceBaseboardsLinear),
                Name = "Replace Baseboards"
            }
        };
    }



    //==============================================================================================
    //=============================================================    List<TestModel>    ==========
    //==============================================================================================

    public List<ProjectTestModel> ValidProjectTestModelList //use this to test "happy path"
    {
        get => new List<ProjectTestModel>() { PatioProjectTestData!, FamilyRoomFlooringProjectTestData!, ReplaceBaseboardsProjectTestData! };
    }

    public ProjectTestModel ValidProjectTestModel => PatioProjectTestData!;



    //==============================================================================================
    //=================================================================    List<Model>    ==========
    //==============================================================================================

    public static List<Project> ProjectsFor(List<ProjectTestModel> testModels) => 
        testModels.Select(testModel => testModel.Project).ToList();
}



//==================================================================================================
//====================================================    Parameterized Test ClassData    ==========
//==================================================================================================

public class ProjectValidClassData : ParameterizedTestClassData
{
    public override IEnumerator<object[]> GetEnumerator() => 
        base.GetEnumerator<ProjectTestModel>(new ProjectTestData().ValidProjectTestModelList);
}
