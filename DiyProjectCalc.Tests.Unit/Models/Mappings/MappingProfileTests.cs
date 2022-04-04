using AutoMapper;
using DiyProjectCalc.Core.Entities.ProjectAggregate;
using DiyProjectCalc.Models.DTO;
using DiyProjectCalc.Models.Mappings;
using DiyProjectCalc.TestHelpers.Helpers;
using DiyProjectCalc.TestHelpers.TestData;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Linq;
using Xunit;

namespace DiyProjectCalc.Tests.Unit.Models.Mappings;
public class MappingProfileTests
{
    [Fact]
    [Trait("AssertConfigurationIsValid", "MappingProfile")]
    public void test_AssertConfigurationIsValid()
    {
        var mapperConfiguration = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
        mapperConfiguration.AssertConfigurationIsValid();
    }

    [Fact]
    [Trait("BasicShape_to_BasicShapeDTO", "MappingProfile")]
    public void MappingProfile_BasicShape_to_BasicShapeDTO()
    {
        //Arrange
        var mapper = MapperHelper.CreateMapper();
        var source = BasicShapeTestData.MockSimpleBasicShape;

        //Act
        var result = mapper.Map<BasicShapeDTO>(source);

        //Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<BasicShapeDTO>();
            result.Id.Should().Be(source.Id);
            result.ShapeType.Should().Be(source.ShapeType);
            result.Name.Should().Be(source.Name);
            result.Number1.Should().Be(source.Number1);
            result.Number2.Should().Be(source.Number2);
            result.Description.Should().Be(source.Description);
            result.Area.Should().Be(source.Area);
            result.Distance.Should().Be(source.Distance);
        }
    }

    [Fact]
    [Trait("BasicShapeDTO_to_BasicShape", "MappingProfile")]
    public void MappingProfile_BasicShapeDTO_to_BasicShape()
    {
        //Arrange
        var mapper = MapperHelper.CreateMapper();
        var source = new BasicShapeDTO(
            Id: 55,
            ShapeType: BasicShapeType.Curved,
            Name: "arc",
            Number1: 11.44,
            Number2: 33.88,
            Description: "nice round shape", 
            Area: 12.33, 
            Distance: 1.2, 
            ProjectName: "fence"
            );

        //Act
        var result = mapper.Map<BasicShape>(source);

        //Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<BasicShape>();
            result.Id.Should().Be(source.Id);
            result.ShapeType.Should().Be(source.ShapeType);
            result.Name.Should().Be(source.Name);
            result.Number1.Should().Be(source.Number1);
            result.Number2.Should().Be(source.Number2);
            result.Description.Should().NotBe(source.Description); //calculated field, should not be mapped
            result.Area.Should().NotBe(source.Area); //calculated field, should not be mapped
            result.Distance.Should().NotBe(source.Distance); //calculated field, should not be mapped
        }
    }

    [Fact]
    [Trait("Project_to_ProjectDTO", "MappingProfile")]
    public void MappingProfile_Project_to_ProjectDTO()
    {
        //Arrange
        var mapper = MapperHelper.CreateMapper();
        var source = ProjectTestData.MockSimpleProject;

        //Act
        var result = mapper.Map<ProjectDTO>(source);

        //Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<ProjectDTO>();
            result.Id.Should().Be(source?.Id);
            result.Name.Should().Be(source?.Name);
        }
    }

    [Fact]
    [Trait("ProjectDTO_to_Project", "MappingProfile")]
    public void MappingProfile_ProjectDTO_to_Project()
    {
        //Arrange
        var mapper = MapperHelper.CreateMapper();
        var source = ProjectTestData.MockSimpleProjectDTO;

        //Act
        var result = mapper.Map<Project>(source);

        //Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<Project>();
            result.Id.Should().Be(source?.Id);
            result.Name.Should().Be(source?.Name);
        }
    }

    [Fact]
    [Trait("Project_to_ProjectDTOWithBasicShapes", "MappingProfile")]
    public void MappingProfile_Project_to_ProjectDTOWithBasicShapes()
    {
        //Arrange
        var mapper = MapperHelper.CreateMapper();
        var source = ProjectTestData.MockSimpleProjectWithBasicShapes;

        //Act
        var result = mapper.Map<ProjectDTOWithBasicShapes>(source);

        //Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<ProjectDTOWithBasicShapes>();
            result.Id.Should().Be(source?.Id);
            result.Name.Should().Be(source?.Name);
            result.BasicShapes.Count().Should().Be(source?.BasicShapes.Count);
        }
    }
}
