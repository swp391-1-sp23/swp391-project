using AutoMapper.Configuration.Annotations;

using SWP391.Project.Entities;

namespace SWP391.Project.Models.Dtos.Account
{
    public class AccountDto : AccountSimplified
    {
        [Ignore]
        public (FileSimplified AvatarInfo, string? AvatarUrl) Avatar { get; set; }
    }
}