using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using SWP391.Project.Entities;
using SWP391.Project.Models.Dtos.Account;

namespace SWP391.Project.MapperProfiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<AccountEntity, AccountDto>();
        }
    }
}