using System.ComponentModel.DataAnnotations;

namespace SWP391.Project.Entities
{
    public class AddressEntity : BaseEntity
    {
        [Required(ErrorMessage = "NAME.VALIDATE.EMPTY")]
        public string AddressName { get; set; } = null!;

        [Required(ErrorMessage = "CITY.VALIDATE.EMPTY")]
        public string City { get; set; } = null!;

        public string? District { get; set; }

        public string? Ward { get; set; }

        [Required(ErrorMessage = "STREET.VALIDATE.EMPTY")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "PRIMARY.VALIDATE.EMPTY")]
        public bool IsPrimary { get; set; }

        public virtual AccountEntity? Account { get; set; } = null!;
    }
}