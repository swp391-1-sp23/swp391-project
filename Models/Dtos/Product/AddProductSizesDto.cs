namespace SWP391.Project.Models.Dtos.Product
{
    public class AddProductSizesDto
    {
        public ICollection<string> SizeNames { get; set; } = null!;
    }
}