using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace SWP391.Project.Entities
{
    [Index(propertyName: nameof(Email), IsUnique = true)]
    public class AccountEntity : BaseEntity
    {
        [Required(ErrorMessage = "EMAIL.VALIDATE.EMPTY")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "FNAME.VALIDATE.EMPTY")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "LNAME.VALIDATE.EMPTY")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "PHONE.VALIDATE.EMPTY")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "ROLE.VALIDATE.EMPTY")]
        public AccountRole Role { get; set; } = AccountRole.Customer;

        [Required(ErrorMessage = "STATUS.VALIDATE.EMPTY")]
        public AccountStatus Status { get; set; } = AccountStatus.Activated;

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        [Required(ErrorMessage = "PASSWORD.VALIDATE.EMPTY")]
        public byte[] Password { get; set; } = null!;

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        [Required(ErrorMessage = "SALT.VALIDATE.EMPTY")]
        public byte[] Salt { get; set; } = null!;

        public virtual ImageEntity? Avatar { get; set; } = new ImageEntity
        {
            BucketName = AvailableBucket.Avatar
        };
    }

    public enum AccountStatus
    {
        Activated,
        Deactivated,
    }

    public enum AccountRole
    {
        Shop,
        Customer,
    }
}