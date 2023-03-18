namespace SWP391.Project.Models.Dtos.Product
{
    public class UpdateProductQuantityDto
    {
        public Guid ProductInStockId { get; set; }
        public Guid SizeId { get; set; }
        public Guid ColorId { get; set; }
        public int Quantity { get; set; }
    }
}