namespace SWP391.Project.Models.Dtos.Product
{
    public class AddCartProductDto
    {
        public Guid ProductInStockId { get; set; }
        public Guid AccountId { get; set; }
        public int Quantity { get; set; }
    }
}