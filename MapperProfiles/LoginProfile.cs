using AutoMapper;

using SWP391.Project.Entities;
using SWP391.Project.Models.Dtos.Login;

namespace SWP391.Project.MapperProfiles
{
    public class LoginProfile : Profile
    {
        public LoginProfile()
        {
            CreateMap<AccountEntity, LoginOutput>();
        }
    }
}