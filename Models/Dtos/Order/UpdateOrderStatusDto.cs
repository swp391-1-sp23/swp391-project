using SWP391.Project.Entities;

namespace SWP391.Project.Models.Dtos.Order
{
    public class UpdateOrderStatusDto
    {
        public Guid Tag { get; set; }
        public OrderStatus Status { get; set; }
    }
}