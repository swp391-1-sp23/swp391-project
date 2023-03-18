namespace SWP391.Project.Entities
{
    public class SizeEntity : BaseEntity
    {
        public string SizeName { get; set; } = string.Empty;
        public virtual ProductEntity? Product { get; set; }
    }

    public class SizeSimplified : BaseSimplified
    {
        public string SizeName { get; set; } = string.Empty;
    }
}