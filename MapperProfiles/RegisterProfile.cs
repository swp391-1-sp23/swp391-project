using System.Text;

using AutoMapper;

using SWP391.Project.Entities;
using SWP391.Project.Models.Dtos.Register;

namespace SWP391.Project.MapperProfiles
{
    public class RegisterProfile : Profile
    {
        public RegisterProfile()
        {
            CreateMap<RegisterInput, AccountEntity>().ForMember(destinationMember: d => d.Password, memberOptions: options => options.ConvertUsing(new PasswordConverter()));
        }
    }

    class PasswordConverter : IValueConverter<string, byte[]>
    {
        public byte[] Convert(string sourceMember, ResolutionContext context)
        {
            return Encoding.UTF8.GetBytes(s: sourceMember);
        }
    }
}