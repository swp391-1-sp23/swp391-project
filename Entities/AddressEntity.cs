namespace SWP391.Project.Entities
{
    public class AddressEntity : BaseEntity
    {
        // [Required(ErrorMessage = "NAME.VALIDATE.EMPTY")]
        public string AddressName { get; set; } = string.Empty;

        // [Required(ErrorMessage = "CITY.VALIDATE.EMPTY")]
        public string City { get; set; } = string.Empty;

        public string? District { get; set; }

        public string? Ward { get; set; }

        // [Required(ErrorMessage = "STREET.VALIDATE.EMPTY")]
        public string Street { get; set; } = string.Empty;

        // [Required(ErrorMessage = "PRIMARY.VALIDATE.EMPTY")]
        public bool IsPrimary { get; set; }

        public virtual AccountEntity? Account { get; set; }
    }

    public class AddressSimplified : BaseSimplified
    {
        public string AddressName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string? District { get; set; } = string.Empty;
        public string? Ward { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
    }
}