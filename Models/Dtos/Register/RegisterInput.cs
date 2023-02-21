using System.ComponentModel.DataAnnotations;
using System.Text;

using AutoMapper;
using AutoMapper.Configuration.Annotations;

namespace SWP391.Project.Models.Dtos.Register
{
    public class RegisterInput
    {
        [Required(ErrorMessage = "EMAIL.VALIDATE.EMPTY")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "PASSWORD.VALIDATE.EMPTY")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "FNAME.VALIDATE.EMPTY")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "LNAME.VALIDATE.EMPTY")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "PHONE.VALIDATE.EMPTY")]
        public string Phone { get; set; } = null!;
    }

}