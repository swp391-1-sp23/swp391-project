namespace SWP391.Project.Models.Dtos.Address
{
    public class AddAddressDto
    {
        public string AddressName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Ward { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
    }
}