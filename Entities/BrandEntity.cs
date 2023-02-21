using System.ComponentModel.DataAnnotations;

namespace SWP391.Project.Entities
{
    public class BrandEntity : BaseEntity
    {
        [Required(ErrorMessage = "NAME.VALIDATE.EMPTY")]
        public string BrandName { get; set; } = null!;

        public virtual ImageEntity? Logo { get; set; } = new ImageEntity
        {
            BucketName = AvailableBucket.Brand
        };
    }
}