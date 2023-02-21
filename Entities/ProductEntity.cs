using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

namespace SWP391.Project.Entities
{
    public class ProductEntity : BaseEntity
    {
        [Required(ErrorMessage = "NAME.VALIDATE.EMPTY")]
        public string ProductName { get; set; } = null!;

        [Precision(precision: 11, scale: 2)]
        [Required(ErrorMessage = "PRICE.VALIDATE.EMPTY")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "DESCRIPTION.VALIDATE.EMPTY")]
        public string Description { get; set; } = null!;

        public virtual ICollection<ImageEntity>? Images { get; set; }
        public virtual BrandEntity? Brand { get; set; } = null!;
    }
}