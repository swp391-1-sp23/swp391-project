namespace SWP391.Project.Models.Dtos.Product
{
    public class AddProductQuantityDto
    {
        public Guid ColorId { get; set; }
        public Guid SizeId { get; set; }
        public int Quantity { get; set; }
    }
}