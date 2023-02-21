namespace SWP391.Project.Entities
{
    public class ImageEntity : BaseEntity
    {
        public AvailableBucket BucketName { get; set; } = AvailableBucket.Misc;
    }

    public enum AvailableBucket
    {
        Avatar,
        Brand,
        Product,
        Misc,
    }
}