using DiyProjectCalc.Models;
using DiyProjectCalc.Tests.TestData.Abstractions;
using DiyProjectCalc.Tests.TestModels;
using System.Collections.Generic;
using System.Linq;

namespace DiyProjectCalc.Tests.TestData;

public class MaterialsTestData
{
    public static readonly double TestPrecision = 0.001;
    public static readonly string NameValid = "Gravel";

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

    public MaterialsTestData() : this(new BasicShapesTestData()) { }

    public MaterialsTestData(BasicShapesTestData basicShapesTestData)
    {
        InitTestModels(basicShapesTestData);
    }


    //==============================================================================================
    //===================================================================    TestModel    ==========
    //==============================================================================================

   
    public MaterialTestModel? GravelTestModel; //use this to test "happy path"
    public MaterialTestModel? EdgingTestModel;
    public MaterialTestModel? PaverTestModel;
    public MaterialTestModel? BaseboardTestModel;
    public MaterialTestModel? UnderlaymentTestModel;
    public MaterialTestModel? FlooringPanelTestModel;

    private void InitTestModels(BasicShapesTestData basicShapesTestData)
    {
        GravelTestModel = new MaterialTestModel()
        {
            TestCaseName = "Gravel",
            Material = new Material()
            {
                Name = NameValid,
                BasicShapes = BasicShapesTestData.BasicShapesFor(basicShapesTestData.TestDataForPatioArea),
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
                BasicShapes = BasicShapesTestData.BasicShapesFor(basicShapesTestData.TestDataForPatioEdgeLinear),
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
                BasicShapes = BasicShapesTestData.BasicShapesFor(basicShapesTestData.TestDataForPatioArea),
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
                BasicShapes = BasicShapesTestData.BasicShapesFor(basicShapesTestData.TestDataForReplaceBaseboardsLinear),
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
                BasicShapes = BasicShapesTestData.BasicShapesFor(basicShapesTestData.TestDataForFamilyRoomFlooringArea),
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
                BasicShapes = BasicShapesTestData.BasicShapesFor(basicShapesTestData.TestDataForFamilyRoomFlooringArea),
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

    //==============================================================================================
    //=================================================================    List<Model>    ==========
    //==============================================================================================

    public static List<Material> MaterialsFor(List<MaterialTestModel> testModels)
    {
        return testModels.Select(testModel => testModel.Material).ToList();
    }

}


//==================================================================================================
//====================================================    Parameterized Test ClassData    ==========
//==================================================================================================

public class MaterialValidClassData : ParameterizedTestClassData
{
    private MaterialsTestData _materialsTestData = new MaterialsTestData();
    public override IEnumerator<object[]> GetEnumerator()
    {
        foreach (var testModel in _materialsTestData.TestDataForPatio) 
        {
            yield return ObjectTo.Test(testModel);
        }
    }
}
