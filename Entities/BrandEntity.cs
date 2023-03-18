namespace SWP391.Project.Entities
{
    public class BrandEntity : BaseEntity
    {
        // [Required(ErrorMessage = "NAME.VALIDATE.EMPTY")]
        public string BrandName { get; set; } = string.Empty;

        public virtual FileEntity? Logo { get; set; } = new FileEntity
        {
            BucketName = AvailableBucket.Brand
        };
    }

    public class BrandSimplified : BaseSimplified
    {
        public string BrandName { get; set; } = string.Empty;
    }
}