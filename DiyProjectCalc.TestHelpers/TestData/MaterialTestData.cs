using DiyProjectCalc.Core.Entities.ProjectAggregate;
using DiyProjectCalc.Infrastructure.Data;
using DiyProjectCalc.TestHelpers.TestData.Abstractions;
using DiyProjectCalc.TestHelpers.TestModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DiyProjectCalc.TestHelpers.TestData;

public class MaterialTestData
{
    public static readonly double TestPrecision = 0.001;
    public static readonly string ValidName = "Gravel";

    public static Material NewMaterial
    {
        get => new Material()
        {
            Name = "New Material",
            MeasurementType = MaterialMeasurement.Linear,
            Length = 8.0,
            Width = 4.0,
            Depth = 0.5
        };
    }

    public static Material MockSimpleMaterial
    {
        get => new Material()
        {
            Id = 67,
            Name = "Material For Testing",
            MeasurementType = MaterialMeasurement.Area,
            Length = 9.9,
            Width = 8.8,
            Depth = 7.7
        };
    }

    public static int MockSimpleMaterialId
    {
        get => MockSimpleMaterial.Id;
    }

    public static Material MockMaterialWithBasicShapes
    {
        get => new Material()
        {
            Id = 67,
            Name = "Material For Testing",
            MeasurementType = MaterialMeasurement.Area,
            Length = 9.9,
            Width = 8.8,
            Depth = 7.7,
            BasicShapes = new List<BasicShape>()
            {
                new BasicShape() 
                { 
                    Id = 12,
                    Name = "twelve",
                    ShapeType = BasicShapeType.Rectangle,
                    Number1 = 1.1,
                    Number2 = 2.2
                },
                new BasicShape()
                {
                    Id = 13,
                    Name = "thirteen",
                    ShapeType = BasicShapeType.Curved,
                    Number1 = 3.3,
                    Number2 = 4.4
                }
            }
        };
    }

    public static int[] MockBasicShapeIdsForMaterialEdit = new int[] { 12, 32, 34 };


    public MaterialTestData() : this(new BasicShapeTestData()) { }

    public MaterialTestData(BasicShapeTestData basicShapesTestData)
    {
        InitTestModels(basicShapesTestData);
    }

    public static int ValidMaterialId(ApplicationDbContext dbContext) =>
        dbContext.Materials.AsNoTracking().FirstOrDefault(m => m.Name == MaterialTestData.ValidName)?.Id ?? 0;

    public static Material? ValidMaterial(ApplicationDbContext dbContext) =>
        dbContext.Materials.AsNoTracking().FirstOrDefault(m => m.Name == MaterialTestData.ValidName);

    public static Material? ValidMaterial(ApplicationDbContext dbContext, int materialId) =>
        dbContext.Materials.AsNoTracking().FirstOrDefault(m => m.Id == materialId);

    public static int[] ValidNewSelectedBasicShapeIds(ApplicationDbContext dbContext) =>
        dbContext.BasicShapes
        .Where(b => BasicShapeTestData.BasicShapeNamesForMaterialEdit.Any(n => n == b.Name))
        .Select(b => b.Id).ToArray();


    //==============================================================================================
    //===================================================================    TestModel    ==========
    //==============================================================================================


    public MaterialTestModel? GravelTestModel; //use this to test "happy path"
    public MaterialTestModel? EdgingTestModel;
    public MaterialTestModel? PaverTestModel;
    public MaterialTestModel? BaseboardTestModel;
    public MaterialTestModel? UnderlaymentTestModel;
    public MaterialTestModel? FlooringPanelTestModel;

    private void InitTestModels(BasicShapeTestData basicShapesTestData)
    {
        GravelTestModel = new MaterialTestModel()
        {
            TestCaseName = "Gravel",
            Material = new Material()
            {
                Name = ValidName,
                BasicShapes = BasicShapeTestData.BasicShapesFor(basicShapesTestData.TestDataForPatioArea),
                MeasurementType = MaterialMeasurement.Volume,
                Length = 3.0,
                Width = 3.0,
                Depth = 3.0,
                DepthNeeded = 2.0
            },
            ExpectedDistanceNeeded = 57.7246,
            ExpectedAreaNeeded = 193.2861,
            ExpectedVolumeNeeded = 386.5722,
            ExpectedQuantityNeeded = 14.3175,
            ExpectedCanCalculateQuantity = true
        };

        EdgingTestModel = new MaterialTestModel()
        {
            TestCaseName = "Landscape Edging",
            Material = new Material()
            {
                Name = "Landscape Edging",
                BasicShapes = BasicShapeTestData.BasicShapesFor(basicShapesTestData.TestDataForPatioEdgeLinear),
                MeasurementType = MaterialMeasurement.Linear,
                Length = 20.0,
                Width = 0.25,
                Depth = 3.0,
                DepthNeeded = 3.0
            },
            ExpectedDistanceNeeded = 57.7246,
            ExpectedAreaNeeded = 163.2861,
            ExpectedVolumeNeeded = 489.8583,
            ExpectedQuantityNeeded = 2.8862,
            ExpectedCanCalculateQuantity = true
        };

        PaverTestModel = new MaterialTestModel()
        {
            TestCaseName = "Paver",
            Material = new Material()
            {
                Name = "Paver",
                BasicShapes = BasicShapeTestData.BasicShapesFor(basicShapesTestData.TestDataForPatioArea),
                MeasurementType = MaterialMeasurement.Area,
                Length = 8.0,
                Width = 4.0,
                Depth = 2.5,
                DepthNeeded = 2.5
            },
            ExpectedDistanceNeeded = 57.7246,
            ExpectedAreaNeeded = 193.2861,
            ExpectedVolumeNeeded = 483.2153,
            ExpectedQuantityNeeded = 6.0401,
            ExpectedCanCalculateQuantity = true
        };

        BaseboardTestModel = new MaterialTestModel()
        {
            TestCaseName = "One Baseboard",
            Material = new Material()
            {
                Name = "One Baseboard",
                BasicShapes = BasicShapeTestData.BasicShapesFor(basicShapesTestData.TestDataForReplaceBaseboardsLinear),
                MeasurementType = MaterialMeasurement.Linear,
                Length = 8.0,
                Width = 1.0,
                Depth = 1.0,
                DepthNeeded = 1.0
            },
            ExpectedDistanceNeeded = 24.0,
            ExpectedAreaNeeded = 24.0,
            ExpectedVolumeNeeded = 24.0,
            ExpectedQuantityNeeded = 3.0,
            ExpectedCanCalculateQuantity = true
        };

        UnderlaymentTestModel = new MaterialTestModel()
        {
            TestCaseName = "Underlayment",
            Material = new Material()
            {
                Name = "Underlayment",
                BasicShapes = BasicShapeTestData.BasicShapesFor(basicShapesTestData.TestDataForFamilyRoomFlooringArea),
                MeasurementType = MaterialMeasurement.Area,
                Length = 30.0,
                Width = 10.0,
                Depth = 1.0,
                DepthNeeded = 1.0
            },
            ExpectedDistanceNeeded = 30.1566,
            ExpectedAreaNeeded = 185,
            ExpectedVolumeNeeded = 185.0,
            ExpectedQuantityNeeded = 0.6167,
            ExpectedCanCalculateQuantity = true
        };

        FlooringPanelTestModel = new MaterialTestModel()
        {
            TestCaseName = "Flooring panel",
            Material = new Material()
            {
                Name = "One panel of flooring",
                BasicShapes = BasicShapeTestData.BasicShapesFor(basicShapesTestData.TestDataForFamilyRoomFlooringArea),
                MeasurementType = MaterialMeasurement.Area,
                Length = 3.0,
                Width = 0.5,
                Depth = 1.0,
                DepthNeeded = 1.0
            },
            ExpectedDistanceNeeded = 30.1566,
            ExpectedAreaNeeded = 185,
            ExpectedVolumeNeeded = 185.0,
            ExpectedQuantityNeeded = 123.3333,
            ExpectedCanCalculateQuantity = true
        };
    }


    //==============================================================================================
    //=============================================================    List<TestModel>    ==========
    //==============================================================================================

    public List<MaterialTestModel> TestDataForPatio //use this to test "happy path"
    {
        get => new List<MaterialTestModel>() { GravelTestModel!, PaverTestModel!, EdgingTestModel! };
    }

    public List<MaterialTestModel> TestDataForReplaceBaseboards
    {
        get => new List<MaterialTestModel>() { BaseboardTestModel! };
    }

    public List<MaterialTestModel> TestDataForFamilyRoomFlooring
    {
        get => new List<MaterialTestModel>() { UnderlaymentTestModel!, FlooringPanelTestModel! };
    }

    public List<MaterialTestModel> TestDataAllCombined
    {
        get => TestDataForPatio
            .Union(TestDataForReplaceBaseboards)
            .Union(TestDataForFamilyRoomFlooring)
            .ToList();
    }

    public List<MaterialTestModel> ValidMaterialTestModelList => TestDataForPatio!;

    public MaterialTestModel ValidMaterialTestModel => GravelTestModel!;


    //==============================================================================================
    //=================================================================    List<Model>    ==========
    //==============================================================================================

    public static List<Material> MaterialsFor(List<MaterialTestModel> testModels) => 
        testModels.Select(testModel => testModel.Material).ToList();

}


//==================================================================================================
//====================================================    Parameterized Test ClassData    ==========
//==================================================================================================

public class MaterialValidClassData : ParameterizedTestClassData
{
    public override IEnumerator<object[]> GetEnumerator() =>
        base.GetEnumerator<MaterialTestModel>(new MaterialTestData().ValidMaterialTestModelList);
}
