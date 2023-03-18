using AutoMapper;

using SWP391.Project.Entities;

namespace SWP391.Project.MapperProfiles
{
    public class ColorProfile : Profile
    {
        public ColorProfile()
        {
            CreateMap<ColorEntity, ColorSimplified>();
        }
    }
}