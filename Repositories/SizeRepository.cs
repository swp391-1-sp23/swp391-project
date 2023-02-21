using SWP391.Project.DbContexts;
using SWP391.Project.Entities;

namespace SWP391.Project.Repositories
{
    public interface ISizeRepository : IBaseRepository<SizeEntity>
    {
    }

    public class SizeRepository : BaseRepository<SizeEntity>, ISizeRepository
    {
        public SizeRepository(ProjectDbContext dbContext) : base(dbContext)
        {
        }
    }
}