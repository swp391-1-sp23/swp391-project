using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391.Project.Entities
{
    public class FileEntity : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public new Guid Id { get; set; } = Guid.NewGuid();
        public AvailableBucket BucketName { get; set; } = AvailableBucket.Misc;
        public string FileName { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
    }

    public class FileSimplified : BaseSimplified
    {
        public AvailableBucket BucketName { get; set; } = AvailableBucket.Misc;
        public string FileName { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
    }

    public enum AvailableBucket
    {
        Avatar,
        Brand,
        Product,
        Misc,
    }
}