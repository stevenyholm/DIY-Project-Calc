using DiyProjectCalc.Models;
using DiyProjectCalc.Tests.TestData.Abstractions;
using DiyProjectCalc.Tests.TestModels;
using System.Collections.Generic;
using System.Linq;

namespace DiyProjectCalc.Tests.TestData;

public class BasicShapesTestData
{
    public static readonly double TestPrecision = 0.001;
    public static readonly string NameValid = "Patio Garage Side Area";
    public static readonly List<string> BasicShapeNamesForMaterialEdit = new List<string>()
    { "Patio Garage Side Area", "Patio Driveway Area",  "Patio Garage Connector Edge"};

    public static BasicShape NewBasicShape 
        {
            get => new BasicShape()
            {
                Name = "New BasicShape",
                ShapeType = BasicShapeType.Rectangle,
                Number1 = 12.0,
                Number2 = 12.0
            };
        }


    //==============================================================================================
    //===================================================================    TestModel    ==========
    //==============================================================================================

    //use this to test "happy path"
    public BasicShapeTestModel PatioGarageSideTestModel = new BasicShapeTestModel()
    {
        TestCaseName = "Patio Garage Side Area",
        BasicShape = new BasicShape()
        {
            Name = NameValid,
            ShapeType = BasicShapeType.Rectangle,
            Number1 = 15.0,
            Number2 = 3.0
        },
        ExpectedDistance = 15.0, 
        ExpectedArea = 45.0
    };

    public BasicShapeTestModel PatioDrivewayTestModel = new BasicShapeTestModel()
    {
        TestCaseName = "Patio Driveway Area",
        BasicShape = new BasicShape()
        {
            Name = "Patio Driveway Area",
            ShapeType = BasicShapeType.Rectangle,
            Number1 = 7.0,
            Number2 = 3.0
        },
        ExpectedDistance = 7.0,
        ExpectedArea = 21.0
    };

    public BasicShapeTestModel PatioCentralCircleTestModel = new BasicShapeTestModel()
    {
        TestCaseName = "Patio Central Circle Area",
        BasicShape = new BasicShape()
        {
            Name = "Patio Central Circle Area",
            ShapeType = BasicShapeType.Curved,
            Number1 = 8.0,
            Number2 = 210.0
        },
        ExpectedDistance = 29.3215,
        ExpectedArea = 117.2861
    };

    public BasicShapeTestModel PatioWedgeTestModel = new BasicShapeTestModel()
    {
        TestCaseName = "Patio Wedge Area",
        BasicShape = new BasicShape()
        {
            Name = "Patio Wedge Area",
            ShapeType = BasicShapeType.Triangle,
            Number1 = 4.0,
            Number2 = 5.0
        },
        ExpectedDistance = 6.4031, 
        ExpectedArea = 10.0
    };

    public BasicShapeTestModel PatioGarageConnectorTestModel = new BasicShapeTestModel()
    {
        TestCaseName = "Patio Garage Connector Edge",
        BasicShape = new BasicShape()
        {
            Name = "Patio Garage Connector Edge",
            ShapeType = BasicShapeType.Rectangle,
            Number1 = 5.0,
            Number2 = 1.0
        },
        ExpectedDistance = 5.0,
        ExpectedArea = 5.0
    };

    public BasicShapeTestModel PatioDrivewayConnectorTestModel = new BasicShapeTestModel()
    {
        TestCaseName = "Patio Driveway Connector Edge",
        BasicShape = new BasicShape()
        {
            Name = "Patio Driveway Connector Edge",
            ShapeType = BasicShapeType.Rectangle,
            Number1 = 10.0,
            Number2 = 1.0
        },
        ExpectedDistance = 10.0,
        ExpectedArea = 10.0
    };

    public BasicShapeTestModel FamilyRoomCenterTestModel = new BasicShapeTestModel()
    {
        TestCaseName = "Family Room Center Area",
        BasicShape = new BasicShape()
        {
            Name = "Family Room Center Area",
            ShapeType = BasicShapeType.Rectangle,
            Number1 = 15.0,
            Number2 = 10.0
        },
        ExpectedDistance = 15.0, 
        ExpectedArea = 150.0
    };

    public BasicShapeTestModel FamilyRoomFireplaceCornerTestModel = new BasicShapeTestModel()
    {
        TestCaseName = "Family Room Fireplace Corner Area",
        BasicShape = new BasicShape()
        {
            Name = "Family Room Fireplace Corner Area",
            ShapeType = BasicShapeType.Triangle,
            Number1 = 9.0,
            Number2 = 3.5
        },
        ExpectedDistance = 9.6566,
        ExpectedArea = 15.75
    };

    public BasicShapeTestModel FamilyRoomFutonSideTestModel = new BasicShapeTestModel()
    {
        TestCaseName = "Family Room Futon Side Area",
        BasicShape = new BasicShape()
        {
            Name = "Family Room Futon Side Area",
            ShapeType = BasicShapeType.Rectangle,
            Number1 = 5.5,
            Number2 = 3.5
        },
        ExpectedDistance = 5.5,
        ExpectedArea = 19.25
    };

    public BasicShapeTestModel BaseboardsLeftOfDoorTestModel = new BasicShapeTestModel()
    {
        TestCaseName = "Baseboards Left Of Door Edge",
        BasicShape = new BasicShape()
        {
            Name = "Baseboards Left Of Door Edge",
            ShapeType = BasicShapeType.Rectangle,
            Number1 = 9.0,
            Number2 = 1.0
        },
        ExpectedDistance = 9.0,
        ExpectedArea = 9.0
    };

    public BasicShapeTestModel BaseboardsRightOfDoorTestModel = new BasicShapeTestModel()
    {
        TestCaseName = "Baseboards Right Of Door Edge",
        BasicShape = new BasicShape()
        {
            Name = "Baseboards Right Of Door Edge",
            ShapeType = BasicShapeType.Rectangle,
            Number1 = 15.0,
            Number2 = 1.0
        },
        ExpectedDistance = 15.0,
        ExpectedArea = 15.0
    };


    //==============================================================================================
    //=============================================================    List<TestModel>    ==========
    //==============================================================================================

    public List<BasicShapeTestModel> TestDataForPatioArea //use this to test "happy path"
    {
        get => new List<BasicShapeTestModel>() { PatioGarageSideTestModel, PatioDrivewayTestModel, PatioCentralCircleTestModel, PatioWedgeTestModel };
    }

    public List<BasicShapeTestModel> TestDataForPatioEdgeLinear
    {
        get => new List<BasicShapeTestModel>() { PatioGarageConnectorTestModel, PatioDrivewayTestModel, PatioDrivewayConnectorTestModel, PatioCentralCircleTestModel, PatioWedgeTestModel };
    }

    public List<BasicShapeTestModel> TestDataForPatioCombined
    {
        get => TestDataForPatioArea
            .Union(TestDataForPatioEdgeLinear)
            .ToList();
    }

    public List<BasicShapeTestModel> TestDataForFamilyRoomFlooringArea
    {
        get => new List<BasicShapeTestModel>() { FamilyRoomCenterTestModel, FamilyRoomFireplaceCornerTestModel, FamilyRoomFutonSideTestModel };
    }

    public List<BasicShapeTestModel> TestDataForReplaceBaseboardsLinear
    {
        get => new List<BasicShapeTestModel>() { BaseboardsLeftOfDoorTestModel, BaseboardsRightOfDoorTestModel };
    }

    public List<BasicShapeTestModel> TestDataForAllCombined
    {
        get => TestDataForPatioCombined
            .Union(TestDataForFamilyRoomFlooringArea)
            .Union(TestDataForReplaceBaseboardsLinear)
            .ToList();
    }



    //==============================================================================================
    //=================================================================    List<Model>    ==========
    //==============================================================================================


    public static List<BasicShape> BasicShapesFor(List<BasicShapeTestModel> testModels) =>
        testModels.Select(testModel => testModel.BasicShape).ToList();

}


//==================================================================================================
//====================================================    Parameterized Test ClassData    ==========
//==================================================================================================

public class BasicShapeValidClassData : ParameterizedTestClassData
{
    public override IEnumerator<object[]> GetEnumerator() => 
        base.GetEnumerator<BasicShapeTestModel>(new BasicShapesTestData().TestDataForPatioArea);
}
