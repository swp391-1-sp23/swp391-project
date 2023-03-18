using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

namespace SWP391.Project.Entities
{
    public class FeedbackEntity : BaseEntity
    {
        // [Required(ErrorMessage = "CONTENT.VALIDATE.EMPTY")]
        public string Content { get; set; } = string.Empty;

        [Precision(precision: 2, scale: 1)]
        // [Required(ErrorMessage = "RATE.VALIDATE.EMPTY")]
        public decimal Rate { get; set; }

        public virtual OrderEntity? Order { get; set; }
    }

    public class FeedbackSimplified : BaseSimplified
    {
        public string Content { get; set; } = string.Empty;
        [Precision(precision: 2, scale: 1)]
        public decimal Rate { get; set; }
    }
}