using AutoMapper;

using SWP391.Project.Entities;
using SWP391.Project.Models.Dtos.Brand;

namespace SWP391.Project.MapperProfiles
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            _ = CreateMap<BrandEntity, BrandSimplified>();
            _ = CreateMap<BrandEntity, BrandDto>().ForMember(destinationMember: destination => destination.Logo, memberOptions: options => options.Ignore());
            _ = CreateMap<AddBrandDto, BrandEntity>();
        }
    }
}