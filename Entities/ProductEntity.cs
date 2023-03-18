using Microsoft.EntityFrameworkCore;

namespace SWP391.Project.Entities
{
    public class ProductEntity : BaseEntity
    {
        // [Required(ErrorMessage = "NAME.VALIDATE.EMPTY")]
        public string ProductName { get; set; } = string.Empty;

        [Precision(precision: 11, scale: 2)]
        // [Required(ErrorMessage = "PRICE.VALIDATE.EMPTY")]
        public decimal Price { get; set; }

        // [Required(ErrorMessage = "DESCRIPTION.VALIDATE.EMPTY")]
        public string Description { get; set; } = string.Empty;

        public virtual ICollection<FileEntity>? Images { get; set; }
        public virtual BrandEntity? Brand { get; set; }
    }

    public class ProductSimplified : BaseSimplified
    {
        public string ProductName { get; set; } = string.Empty;
        [Precision(precision: 11, scale: 2)]
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}