using System.ComponentModel.DataAnnotations;

namespace SWP391.Project.Models.Dtos.Login
{
    public class LoginInput
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(length: 6)]
        public string Password { get; set; } = null!;
    }
}