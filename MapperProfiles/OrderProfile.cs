using AutoMapper;

using SWP391.Project.Entities;
using SWP391.Project.Models.Dtos.Order;

namespace SWP391.Project.MapperProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            _ = CreateMap<OrderEntity, OrderDto>()
                .ForMember(destinationMember: destination => destination.Product, memberOptions: options => options.Ignore())
                .ForMember(destinationMember: destination => destination.Color, memberOptions: options => options.Ignore())
                .ForMember(destinationMember: destination => destination.Size, memberOptions: options => options.Ignore())
                .ForMember(destinationMember: destination => destination.Address, memberOptions: options => options.Ignore());

            _ = CreateMap<OrderEntity, OrderSimplified>();
        }
    }
}