using System.Text.Json.Serialization;

using Microsoft.EntityFrameworkCore;

namespace SWP391.Project.Entities
{
    [Index(propertyName: nameof(Email), IsUnique = true)]
    public class AccountEntity : BaseEntity
    {
        // [Required(ErrorMessage = "EMAIL.VALIDATE.EMPTY")]
        public string Email { get; set; } = string.Empty;

        // [Required(ErrorMessage = "FNAME.VALIDATE.EMPTY")]
        public string FirstName { get; set; } = string.Empty;

        // [Required(ErrorMessage = "LNAME.VALIDATE.EMPTY")]
        public string LastName { get; set; } = string.Empty;

        // [Required(ErrorMessage = "PHONE.VALIDATE.EMPTY")]
        public string Phone { get; set; } = string.Empty;

        // [Required(ErrorMessage = "ROLE.VALIDATE.EMPTY")]
        public AccountRole Role { get; set; } = AccountRole.Customer;

        // [Required(ErrorMessage = "STATUS.VALIDATE.EMPTY")]
        public AccountStatus Status { get; set; } = AccountStatus.Activated;

        [JsonIgnore]
        // [Required(ErrorMessage = "PASSWORD.VALIDATE.EMPTY")]
        public byte[]? Password { get; set; }

        [JsonIgnore]
        // [Required(ErrorMessage = "SALT.VALIDATE.EMPTY")]
        public byte[]? Salt { get; set; }

        public virtual FileEntity? Avatar { get; set; } = new FileEntity
        {
            BucketName = AvailableBucket.Avatar
        };
    }

    public class AccountSimplified : BaseSimplified
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public AccountRole Role { get; set; } = AccountRole.Customer;
        public AccountStatus Status { get; set; } = AccountStatus.Activated;
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