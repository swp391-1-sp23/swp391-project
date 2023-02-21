using SWP391.Project.DbContexts;
using SWP391.Project.Entities;

namespace SWP391.Project.Repositories
{
    public interface IBrandRepository : IBaseRepository<BrandEntity>
    {
    }

    public class BrandRepository : BaseRepository<BrandEntity>, IBrandRepository
    {
        public BrandRepository(ProjectDbContext dbContext) : base(dbContext)
        {
        }
    }
}