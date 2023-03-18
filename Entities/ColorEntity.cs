namespace SWP391.Project.Entities
{
    public class ColorEntity : BaseEntity
    {
        public string ColorName { get; set; } = string.Empty;
        public virtual ProductEntity? Product { get; set; }
    }

    public class ColorSimplified : BaseSimplified
    {
        public string ColorName { get; set; } = string.Empty;
    }
}