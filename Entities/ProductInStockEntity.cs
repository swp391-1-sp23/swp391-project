namespace SWP391.Project.Entities
{
    public class ProductInStockEntity : BaseEntity
    {
        public int Quantity { get; set; }
        public virtual ColorEntity? Color { get; set; } = null!;
        public virtual SizeEntity? Size { get; set; } = null!;
        public virtual ProductEntity? Product { get; set; } = null!;
    }
}