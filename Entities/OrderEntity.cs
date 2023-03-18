namespace SWP391.Project.Entities
{
    public class OrderEntity : BaseEntity
    {
        public Guid Tag { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Created;
        public virtual ProductInStockEntity? Product { get; set; }
        public virtual AddressEntity? ShipmentAddress { get; set; }
    }

    public class OrderSimplified : BaseSimplified
    {
        public Guid Tag { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Created;
    }

    public enum OrderStatus
    {
        Created,
        Paid,
        Shipped,
    }
}