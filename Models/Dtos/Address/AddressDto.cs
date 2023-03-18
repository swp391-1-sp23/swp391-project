using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper.Configuration.Annotations;

using SWP391.Project.Entities;

namespace SWP391.Project.Models.Dtos.Address
{
    public class AddressDto : AddressSimplified
    {
        [Ignore]
        public AccountSimplified Account { get; set; } = null!;
    }
}