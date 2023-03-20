namespace SWP391.Project.Models.Dtos.Image
{
    public class AddProductImagesDto
    {
        // public Guid ProductId { get; set; }
        public IFormFileCollection Images { get; set; } = null!;
    }
}