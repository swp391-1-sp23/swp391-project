using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391.Project.Entities
{
    public class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        // [Required(ErrorMessage = "CREATED.VALIDATE.EMPTY")]
        public DateTime CreatedAt { get; init; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;
    }

    public class BaseSimplified
    {
        public Guid Id { get; set; }
        // public DateTime CreatedAt { get; init; } = DateTime.Now;
        // public bool IsDeleted { get; set; } = false;
    }
}