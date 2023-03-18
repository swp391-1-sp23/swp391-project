namespace SWP391.Project.Models.Dtos.Order
{
    public class AddOrderDto
    {
        public Guid ShippingAddressId { get; set; }
        // key: cartProductId
        public ICollection<Guid> ProductInStocks { get; set; } = null!;
    }
}