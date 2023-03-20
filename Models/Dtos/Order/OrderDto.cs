using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper.Configuration.Annotations;

using SWP391.Project.Entities;

namespace SWP391.Project.Models.Dtos.Order
{
    public class OrderDto : OrderSimplified
    {
        [Ignore]
        public ProductSimplified Product { get; set; } = null!;
        [Ignore]
        public ColorSimplified Color { get; set; } = null!;
        [Ignore]
        public SizeSimplified Size { get; set; } = null!;
        [Ignore]
        public AddressSimplified Address { get; set; } = null!;
    }
}