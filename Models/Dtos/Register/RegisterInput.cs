using System.ComponentModel.DataAnnotations;

namespace SWP391.Project.Models.Dtos.Register
{
    public class RegisterInput
    {
        [Required(ErrorMessage = "EMAIL.VALIDATE.EMPTY")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "ROLE.VALIDATE.EMPTY")]
        public string Role { get; set; } = null!;

        [Required(ErrorMessage = "PASSWORD.VALIDATE.EMPTY")]
        public string Password { get; set; } = null!;
    }
}