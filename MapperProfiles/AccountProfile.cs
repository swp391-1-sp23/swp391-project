using AutoMapper;

using SWP391.Project.Entities;
using SWP391.Project.Models.Dtos.Account;

namespace SWP391.Project.MapperProfiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            _ = CreateMap<AccountEntity, AccountDto>().ForMember(destinationMember: destination => destination.Avatar, memberOptions: options => options.Ignore());
            _ = CreateMap<AccountEntity, AccountSimplified>();
            _ = CreateMap<UpdateAccountDto, AccountEntity>();
        }
    }
}