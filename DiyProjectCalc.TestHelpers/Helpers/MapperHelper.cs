using AutoMapper;
using DiyProjectCalc.Models.Mappings;

namespace DiyProjectCalc.TestHelpers.Helpers;
public static class MapperHelper
{
    public static IMapper CreateMapper() =>
        new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); }).CreateMapper();
}
