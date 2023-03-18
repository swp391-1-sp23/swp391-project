namespace SWP391.Project.Models.Dtos.Feedback
{
    public class AddFeedbackDto
    {
        public Guid OrderId { get; set; }
        public string Content { get; set; } = string.Empty;
        public decimal Rate { get; set; } = 5;
    }
}
