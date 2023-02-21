namespace SWP391.Project.Models.Dtos.Product
{
    public class AddProductDto
    {
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;
    }
}