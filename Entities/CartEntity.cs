namespace SWP391.Project.Entities
{
    public class CartEntity : BaseEntity
    {
        public int Quantity { get; set; }
        public virtual AccountEntity? Account { get; set; }
        public virtual ProductInStockEntity? Product { get; set; }
    }

    public class CartSimplified : BaseSimplified
    {
        public int Quantity { get; set; }
    }
}