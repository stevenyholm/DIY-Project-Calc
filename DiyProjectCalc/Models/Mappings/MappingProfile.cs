using AutoMapper;
using DiyProjectCalc.Core.Entities.ProjectAggregate;
using DiyProjectCalc.Models.DTO;

namespace DiyProjectCalc.Models.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BasicShape, BasicShape>()
            .ForMember(dest => dest.Materials, opt => opt.Ignore())
            .AfterMap((source, dest, resolutionContext) =>
            {
                foreach (var material in source.Materials)
                    dest.AddMaterial(material);
            });

        CreateMap<BasicShape, BasicShapeDTO>()
            .ReverseMap();

        CreateMap<Material, Material>()
            .ForMember(dest => dest.BasicShapes, opt => opt.Ignore())
            .AfterMap((source, dest, resolutionContext) =>
            {
                foreach(var basicShape in source.BasicShapes)
                    dest.AddBasicShape(basicShape);
            });

        CreateMap<Project, ProjectDTOWithBasicShapes>();

        CreateMap<Project, ProjectDTO>()
            .ReverseMap();
    }
}