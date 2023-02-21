namespace SWP391.Project.Entities
{
    public class SizeEntity : BaseEntity
    {
        public string SizeName { get; set; } = null!;
        public virtual ProductEntity? Product { get; set; } = null!;
    }
}