using AutoMapper;
using DiyProjectCalc.Models.DTO;

namespace DiyProjectCalc.Models.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BasicShape, BasicShapeDTO>()
            .ReverseMap();

        CreateMap<Project, ProjectDTOWithBasicShapes>();

        CreateMap<Project, ProjectDTO>()
            .ReverseMap();

    }
}