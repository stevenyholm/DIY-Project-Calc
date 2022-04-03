using Moq;
using SUT = DiyProjectCalc.Controllers.API;
using System.Threading.Tasks;
using Xunit;
using DiyProjectCalc.TestHelpers.TestData;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using DiyProjectCalc.Models.DTO;
using FluentAssertions;
using DiyProjectCalc.TestHelpers.Helpers;
using DiyProjectCalc.TestHelpers.UnitTestBase;
using DiyProjectCalc.Core.Entities.ProjectAggregate.Specifications;
using System.Linq;

namespace DiyProjectCalc.Tests.Unit.Controllers.API;
public class BasicShapesControllerTests : BaseControllerTests
{
    private SUT.BasicShapesController _controller;
    public BasicShapesControllerTests()
    {
        _controller = new SUT.BasicShapesController(
            MapperHelper.CreateMapper(), 
            base._mockProjectRepository.Object
            );
    }

    [Fact]
    [Trait("GetAllForProject", "GET")]
    public async Task ValidProjectId_Returns_BasicShapes_For_GetAllForProject()
    {
        //Arrange
        var expectedProject = ProjectTestData.MockSimpleProjectWithBasicShapes;
        base._mockProjectRepository.Setup(r => r.GetBySpecAsync(It.IsAny<ProjectWithBasicShapesSpec>(), TestCancellationToken()))
            .ReturnsAsync(expectedProject);

        //Act
        var result = await _controller.GetAllForProject(expectedProject.Id);

        //Assert
        using (new AssertionScope())
        {
            result.As<ProjectDTOWithBasicShapes>().BasicShapes.Should().HaveCount(expectedProject.BasicShapes.Count);
            result.As<ProjectDTOWithBasicShapes>().Id.Should().Be(expectedProject.Id);
            result.As<ProjectDTOWithBasicShapes>().Name.Should().Be(expectedProject.Name);
        }
    }

    [Fact]
    [Trait("Get_BasicShapeId", "GET")]
    public async Task ValidBasicShapeId_Returns_BasicShape_For_Get()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProjectWithBasicShapes;
        var expectedBasicShape = project.BasicShapes.First();
        base._mockProjectRepository.Setup(r => r.GetBySpecAsync(It.IsAny<ProjectWithBasicShapesSpec>(), TestCancellationToken()))
            .ReturnsAsync(project);

        //Act
        var result = await _controller.Get(project.Id, expectedBasicShape.Id);

        //Assert
        result.As<BasicShapeDTO>().Id.Should().Be(expectedBasicShape.Id);
    }

    [Fact]
    [Trait("Post_BasicShape", "POST")]
    public async Task ValidBasicShape_Returns_CreatedAtActionResult_For_Post()
    {
        //Arrange
        var projectId = ProjectTestData.MockSimpleProjectId;
        var newBasicShapeDTO = BasicShapeTestData.NewBasicShapeDTOWithProjectId(projectId);

        //Act
        var result = await _controller.Post(projectId, newBasicShapeDTO);

        //Assert
        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    [Trait("Put_BasicShape", "PUT")]
    public async Task ValidBasicShape_Returns_Ok_For_Put()
    {
        //Arrange
        var projectId = ProjectTestData.MockSimpleProjectId;
        var editedBasicShapeDTO = BasicShapeTestData.MockSimpleBasicShapeDTO;

        //Act
        var result = await _controller.Put(projectId, editedBasicShapeDTO.Id, editedBasicShapeDTO);

        //Assert
        result.Should().BeOfType<OkResult>();
    }

    [Fact]
    [Trait("Delete_BasicShapeId", "DELETE")]
    public async Task ValidBasicShapeId_Returns_ProjectId_For_Delete()
    {
        //Arrange
        var expectedProject = ProjectTestData.MockSimpleProjectWithBasicShapes;
        base._mockProjectRepository.Setup(r => r.GetBySpecAsync(It.IsAny<ProjectWithBasicShapesSpec>(), TestCancellationToken()))
            .ReturnsAsync(expectedProject);
        var deletedBasicShape = expectedProject.BasicShapes.First();

        //Act
        var result = await _controller.Delete(expectedProject.Id, deletedBasicShape.Id);

        //Assert
        result.Should().BeOfType<OkResult>();
    }

}
