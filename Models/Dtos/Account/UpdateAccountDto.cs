using System.ComponentModel.DataAnnotations;

namespace SWP391.Project.Models.Dtos.Account
{
    public class UpdateAccountDto
    {
        public Guid AccountId { get; set; }
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        [Phone]
        public string Phone { get; set; } = string.Empty;
    }
}