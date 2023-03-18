namespace SWP391.Project.Models.Dtos.Product
{
    public class AddProductColorsDto
    {
        public ICollection<string> ColorNames { get; set; } = null!;
    }
}