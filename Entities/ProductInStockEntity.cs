namespace SWP391.Project.Entities
{
    public class ProductInStockEntity : BaseEntity
    {
        public int Quantity { get; set; }
        public virtual ColorEntity? Color { get; set; }
        public virtual SizeEntity? Size { get; set; }
        public virtual ProductEntity? Product { get; set; }
    }

    public class ProductInStockSimplified : BaseSimplified
    {
        public int Quantity { get; set; }
    }
}