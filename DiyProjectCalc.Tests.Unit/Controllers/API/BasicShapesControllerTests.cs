using Moq;
using SUT = DiyProjectCalc.Controllers.API;
using System.Threading.Tasks;
using DiyProjectCalc.Repositories;
using Xunit;
using DiyProjectCalc.TestHelpers.TestData;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using DiyProjectCalc.Models.DTO;
using FluentAssertions;
using DiyProjectCalc.TestHelpers.Helpers;

namespace DiyProjectCalc.Tests.Unit.Controllers.API;
public class BasicShapesControllerTests
{
    private Mock<IBasicShapeRepository> _mockRepository = new Mock<IBasicShapeRepository>();
    private Mock<IProjectRepository> _mockProjectRepository = new Mock<IProjectRepository>();
    private SUT.BasicShapesController _controller;
    public BasicShapesControllerTests()
    {
        _controller = new SUT.BasicShapesController(MapperHelper.CreateMapper(), 
            _mockRepository.Object, 
            _mockProjectRepository.Object);
    }

    [Fact]
    [Trait("GetAllForProject", "GET")]
    public async Task ValidProjectId_Returns_BasicShapes_For_GetAllForProject()
    {
        //Arrange
        var expectedProject = ProjectTestData.MockSimpleProjectWithBasicShapes;
        _mockProjectRepository.Setup(r => r.GetProjectWithBasicShapesAsync(It.IsAny<int>())).ReturnsAsync(expectedProject);

        //Act
        var result = await _controller.GetAllForProject(expectedProject.ProjectId);

        //Assert
        using (new AssertionScope())
        {
            result.As<ProjectDTOWithBasicShapes>().BasicShapes.Should().HaveCount(expectedProject.BasicShapes.Count);
            result.As<ProjectDTOWithBasicShapes>().ProjectId.Should().Be(expectedProject.ProjectId);
            result.As<ProjectDTOWithBasicShapes>().Name.Should().Be(expectedProject.Name);
        }
    }

    [Fact]
    [Trait("Get_BasicShapeId", "GET")]
    public async Task ValidBasicShapeId_Returns_BasicShape_For_Get()
    {
        //Arrange
        var expectedBasicShape = new BasicShapeTestData().ValidBasicShapeTestModel!.BasicShape;
        _mockRepository.Setup(r => r.GetBasicShapeAsync(It.IsAny<int>())).ReturnsAsync(expectedBasicShape);

        //Act
        var result = await _controller.Get(expectedBasicShape.BasicShapeId);

        //Assert
        result.As<BasicShapeDTO>().BasicShapeId.Should().Be(expectedBasicShape.BasicShapeId);
    }

    [Fact]
    [Trait("Post_BasicShape", "POST")]
    public async Task ValidBasicShape_Returns_CreatedAtActionResult_For_Post()
    {
        //Arrange
        var projectId = ProjectTestData.MockSimpleProjectId;
        var newBasicShape = BasicShapeTestData.NewBasicShapeDTOWithProjectId(projectId);

        //Act
        var result = await _controller.Post(newBasicShape);

        //Assert
        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    [Trait("Put_BasicShape", "PUT")]
    public async Task ValidBasicShape_Returns_Ok_For_Put()
    {
        //Arrange
        var editedModel = BasicShapeTestData.MockSimpleBasicShapeDTO;

        //Act
        var result = await _controller.Put(editedModel.BasicShapeId, editedModel);

        //Assert
        result.Should().BeOfType<OkResult>();
    }

    [Fact]
    [Trait("Delete_BasicShapeId", "DELETE")]
    public async Task ValidBasicShapeId_Returns_ProjectId_For_Delete()
    {
        //Arrange
        var basicShape = BasicShapeTestData.MockSimpleBasicShape;
        _mockRepository.Setup(r => r.GetBasicShapeAsync(It.IsAny<int>())).ReturnsAsync(basicShape);
        var basicShapeId = BasicShapeTestData.MockSimpleBasicShapeId;

        //Act
        var result = await _controller.Delete(basicShapeId);

        //Assert
        result.As<ActionResult<int>>().Value.Should().Be(basicShape.ProjectId);
    }

}
