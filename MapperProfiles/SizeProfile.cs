using AutoMapper;

using SWP391.Project.Entities;

namespace SWP391.Project.MapperProfiles
{
    public class SizeProfile : Profile
    {
        public SizeProfile()
        {
            CreateMap<SizeEntity, SizeSimplified>();
        }
    }
}