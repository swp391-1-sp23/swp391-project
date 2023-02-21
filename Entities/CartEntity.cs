namespace SWP391.Project.Entities
{
    public class CartEntity : BaseEntity
    {
        public int Quantity { get; set; }
        public virtual AccountEntity? Account { get; set; } = null!;
        public virtual ProductInStockEntity? Product { get; set; } = null!;
    }
}