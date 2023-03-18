using AutoMapper;

using SWP391.Project.Entities;
using SWP391.Project.Models.Dtos.Cart;

namespace SWP391.Project.MapperProfiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            _ = CreateMap<CartEntity, CartDto>()
                .ForMember(destinationMember: destination => destination.Product, memberOptions: options => options.Ignore())
                .ForMember(destinationMember: destination => destination.Color, memberOptions: options => options.Ignore())
                .ForMember(destinationMember: destination => destination.Size, memberOptions: options => options.Ignore());
            _ = CreateMap<CartEntity, CartSimplified>();
        }
    }
}