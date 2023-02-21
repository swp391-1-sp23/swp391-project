namespace SWP391.Project.Entities
{
    public class ColorEntity : BaseEntity
    {
        public string ColorName { get; set; } = null!;
        public virtual ProductEntity? Product { get; set; } = null!;
    }
}