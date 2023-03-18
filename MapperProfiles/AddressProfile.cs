using AutoMapper;

using SWP391.Project.Entities;
using SWP391.Project.Models.Dtos.Address;

namespace SWP391.Project.MapperProfiles
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            _ = CreateMap<AddressEntity, AddressSimplified>();
            _ = CreateMap<AddressEntity, AddressDto>().ForMember(destinationMember: destination => destination.Account, memberOptions: options => options.Ignore());
            _ = CreateMap<AddAddressDto, AddressEntity>();
        }
    }
}