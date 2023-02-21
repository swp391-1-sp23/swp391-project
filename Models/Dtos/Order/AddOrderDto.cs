namespace SWP391.Project.Models.Dtos.Order
{
    public class AddOrderDto
    {
        public Guid ShippingAddressId { get; set; }
        // key: cartProductId | value: quantity
        public ICollection<Guid> Products { get; set; } = null!;
    }
}