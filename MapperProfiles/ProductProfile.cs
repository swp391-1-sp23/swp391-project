using AutoMapper;

using SWP391.Project.Entities;
using SWP391.Project.Models.Dtos.Product;

namespace SWP391.Project.MapperProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            _ = CreateMap<ProductEntity, ProductSimplified>();
            _ = CreateMap<ProductEntity, ProductDto>()
                .ForMember(destinationMember: destination => destination.Brand, memberOptions: options => options.Ignore())
                .ForMember(destinationMember: destination => destination.Colors, memberOptions: options => options.Ignore())
                .ForMember(destinationMember: destination => destination.Images, memberOptions: options => options.Ignore())
                .ForMember(destinationMember: destination => destination.InStocks, memberOptions: options => options.Ignore())
                .ForMember(destinationMember: destination => destination.Sizes, memberOptions: options => options.Ignore());
            _ = CreateMap<AddProductDto, ProductEntity>();
            _ = CreateMap<UpdateProductDto, ProductEntity>();
        }
    }
}
