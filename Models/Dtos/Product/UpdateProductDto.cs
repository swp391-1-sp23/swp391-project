namespace SWP391.Project.Models.Dtos.Product
{
    public class UpdateProductDto
    {
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}