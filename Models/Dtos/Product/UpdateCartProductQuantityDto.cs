namespace SWP391.Project.Models.Dtos.Product
{
    public class UpdateCartProductQuantityDto
    {
        public Guid CartProductId { get; set; }
        public int Quantity { get; set; }
    }
}