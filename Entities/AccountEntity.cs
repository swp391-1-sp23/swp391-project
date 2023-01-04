using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391.Project.Entities
{
    public class AccountEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "EMAIL.VALIDATE.EMPTY")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "ROLE.VALIDATE.EMPTY")]
        public string Role { get; set; } = null!;

        [Required(ErrorMessage = "PASSWORD.VALIDATE.EMPTY")]
        public byte[] Password { get; set; } = null!;

        [Required(ErrorMessage = "SALT.VALIDATE.EMPTY")]
        public byte[] Salt { get; set; } = null!;
    }
}