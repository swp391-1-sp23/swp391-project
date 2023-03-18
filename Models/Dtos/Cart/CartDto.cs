using AutoMapper.Configuration.Annotations;

using SWP391.Project.Entities;

namespace SWP391.Project.Models.Dtos.Cart
{
    public class CartDto : CartSimplified
    {
        // public AccountSimplified Account { get; set; } = null!;
        [Ignore]
        public ProductSimplified Product { get; set; } = null!;
        [Ignore]
        public ColorSimplified Color { get; set; } = null!;
        [Ignore]
        public SizeSimplified Size { get; set; } = null!;
    }
}
