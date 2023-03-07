using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using SWP391.Project.Entities;
using SWP391.Project.Models.Dtos.Order;

namespace SWP391.Project.MapperProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderEntity, OrderDto>();
        }
    }
}