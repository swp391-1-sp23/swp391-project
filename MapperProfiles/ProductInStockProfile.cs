using AutoMapper;

using SWP391.Project.Entities;

namespace SWP391.Project.MapperProfiles
{
    public class ProductInStockProfile : Profile
    {
        public ProductInStockProfile()
        {
            CreateMap<ProductInStockEntity, ProductInStockSimplified>();
        }
    }
}