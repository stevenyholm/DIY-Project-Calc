using AutoMapper;
using DiyProjectCalc.Core.Entities.ProjectAggregate;
using DiyProjectCalc.Models.DTO;

namespace DiyProjectCalc.Models.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BasicShape, BasicShape>();

        CreateMap<BasicShape, BasicShapeDTO>()
            .ReverseMap();

        CreateMap<Material, Material>();

        CreateMap<Project, ProjectDTOWithBasicShapes>();

        CreateMap<Project, ProjectDTO>()
            .ReverseMap();

    }
}