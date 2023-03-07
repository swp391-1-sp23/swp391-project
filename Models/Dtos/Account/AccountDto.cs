using SWP391.Project.Entities;

namespace SWP391.Project.Models.Dtos.Account
{
    public class AccountDto : AccountEntity
    {
        public new string Avatar { get; set; } = string.Empty;
    }
}