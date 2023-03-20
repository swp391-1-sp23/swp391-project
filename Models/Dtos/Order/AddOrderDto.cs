namespace SWP391.Project.Models.Dtos.Order
{
    public class AddOrderDto
    {
        public Guid ShippingAddressId { get; set; }
        // key: cartProductId
        public ICollection<Guid> CartIds { get; set; } = null!;
    }
}