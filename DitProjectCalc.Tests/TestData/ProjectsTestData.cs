using DiyProjectCalc.Models;
using DiyProjectCalc.Tests.TestData.Abstractions;
using DiyProjectCalc.Tests.TestModels;
using System.Collections.Generic;
using System.Linq;

namespace DiyProjectCalc.Tests.TestData;

public class ProjectsTestData
{
    public static readonly string NameValid = "Patio";
    public static readonly int CountBasicShapesForProject = 6;
    public static readonly int CountMaterialsForProject = 3;
    public static readonly int CountAllProjects = 3;

    public static Project NewProject
    {
        get => new Project()
        {
            Name = "New Project"
        };
    }

    public ProjectsTestData()
    {
        var basicShapesTestData = new BasicShapesTestData();
        var materialsTestData = new MaterialsTestData(basicShapesTestData);

        InitTestModels(materialsTestData, basicShapesTestData);
    }

    //==============================================================================================
    //===================================================================    TestModel    ==========
    //==============================================================================================

    
    public ProjectTestModel? PatioProjectTestData;  //use this to test "happy path"
    public ProjectTestModel? FamilyRoomFlooringProjectTestData;
    public ProjectTestModel? ReplaceBaseboardsProjectTestData;

    private void InitTestModels(MaterialsTestData materialsTestData, BasicShapesTestData basicShapesTestData)
    {
        PatioProjectTestData = new ProjectTestModel()
        {
            TestCaseName = "Patio measures in volume",
            Project = new Project()
            {
                Materials = MaterialsTestData.MaterialsFor(materialsTestData.TestDataForPatio),
                BasicShapes = BasicShapesTestData.BasicShapesFor(basicShapesTestData.TestDataForPatioCombined),
                Name = NameValid
            }
        };

        FamilyRoomFlooringProjectTestData = new ProjectTestModel()
        {
            TestCaseName = "Family Room Flooring measures in area",
            Project = new Project()
            {
                Materials = MaterialsTestData.MaterialsFor(materialsTestData.TestDataForFamilyRoomFlooring),
                BasicShapes = BasicShapesTestData.BasicShapesFor(basicShapesTestData.TestDataForFamilyRoomFlooringArea),
                Name = "Family Room Flooring"
            }
        };

        ReplaceBaseboardsProjectTestData = new ProjectTestModel()
        {
            TestCaseName = "Replace Baseboards measures in linear",
            Project = new Project()
            {
                Materials = MaterialsTestData.MaterialsFor(materialsTestData.TestDataForReplaceBaseboards),
                BasicShapes = BasicShapesTestData.BasicShapesFor(basicShapesTestData.TestDataForReplaceBaseboardsLinear),
                Name = "Replace Baseboards"
            }
        };
    }



    //==============================================================================================
    //=============================================================    List<TestModel>    ==========
    //==============================================================================================

    public List<ProjectTestModel> TestDataForAllProjects //use this to test "happy path"
    {
        get => new List<ProjectTestModel>() { PatioProjectTestData!, FamilyRoomFlooringProjectTestData!, ReplaceBaseboardsProjectTestData! };
    }



    //==============================================================================================
    //=================================================================    List<Model>    ==========
    //==============================================================================================

    public static List<Project> ProjectsFor(List<ProjectTestModel> testModels)
    {
        return testModels.Select(testModel => testModel.Project).ToList();
    }
}



//==================================================================================================
//====================================================    Parameterized Test ClassData    ==========
//==================================================================================================

public class ProjectValidClassData : ParameterizedTestClassData
{
    private ProjectsTestData _projectsTestData = new ProjectsTestData();
    public override IEnumerator<object[]> GetEnumerator()
    {
        foreach(var testModel in _projectsTestData.TestDataForAllProjects)
        {
            yield return ObjectTo.Test(testModel);
        }
    }
}
