using AutoMapper.Configuration.Annotations;

using SWP391.Project.Entities;

namespace SWP391.Project.Models.Dtos.Product
{
    public class ProductDto : ProductSimplified
    {
        [Ignore]
        public ICollection<(FileSimplified ImageInfo, string ImageUrl)>? Images { get; set; }
        [Ignore]
        public BrandSimplified Brand { get; set; } = null!;
        // public ICollection<FeedbackSimplified>? Feedbacks { get; set; }
        [Ignore]
        public ICollection<ColorSimplified>? Colors { get; set; }
        [Ignore]
        public ICollection<SizeSimplified>? Sizes { get; set; }
        [Ignore]
        public IDictionary<Guid, (Guid ColorId, Guid SizeId, int Quantity)>? InStocks { get; set; }
    }
}