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
        get
        {
            var project = MockSimpleProject;
            foreach(var basicShape in MockBasicShapesCollection)
                project.AddBasicShape(basicShape);
            return project;
        }
    }

    public static Project MockSimpleProjectWithMaterials
    {
        get
        {
            var project = MockSimpleProject;
            foreach(var material in MockMaterialsCollection)
                project.AddMaterial(material);
            return project;
        }
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
        get => MockSimpleProject.BasicShapes.Count();
    }

    public static int MockSimpleProjectCountMaterials
    {
        get => MockSimpleProject.Materials.Count();
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


    //use this to test "happy path"
    public ProjectTestModel PatioProjectTestData = new ProjectTestModel()
    {
        TestCaseName = "Patio measures in volume",
        Project = new Project()
        {
            Name = ValidName
        }
    };

    public ProjectTestModel FamilyRoomFlooringProjectTestData = new ProjectTestModel()
    {
        TestCaseName = "Family Room Flooring measures in area",
        Project = new Project()
        {
            Name = "Family Room Flooring"
        }
    };

    public ProjectTestModel ReplaceBaseboardsProjectTestData = new ProjectTestModel()
    {
        TestCaseName = "Replace Baseboards measures in linear",
        Project = new Project()
        {
            Name = "Replace Baseboards"
        }
    };

    private void InitTestModels(MaterialTestData materialsTestData, BasicShapeTestData basicShapesTestData)
    {
        PatioProjectTestData.AddBasicShapes(BasicShapeTestData.BasicShapesFor(
            basicShapesTestData.TestDataForPatioCombined));
        PatioProjectTestData.AddMaterials(MaterialTestData.MaterialsFor(
            materialsTestData.TestDataForPatio));

        FamilyRoomFlooringProjectTestData.AddBasicShapes(BasicShapeTestData.BasicShapesFor(
            basicShapesTestData.TestDataForFamilyRoomFlooringArea));
        FamilyRoomFlooringProjectTestData.AddMaterials(MaterialTestData.MaterialsFor(
            materialsTestData.TestDataForFamilyRoomFlooring));

        ReplaceBaseboardsProjectTestData.AddBasicShapes(BasicShapeTestData.BasicShapesFor(
            basicShapesTestData.TestDataForReplaceBaseboardsLinear));
        ReplaceBaseboardsProjectTestData.AddMaterials(MaterialTestData.MaterialsFor(
            materialsTestData.TestDataForReplaceBaseboards));
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
