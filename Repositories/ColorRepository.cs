using SWP391.Project.DbContexts;
using SWP391.Project.Entities;

namespace SWP391.Project.Repositories
{
    public interface IColorRepository : IBaseRepository<ColorEntity>
    {
    }

    public class ColorRepository : BaseRepository<ColorEntity>, IColorRepository
    {
        public ColorRepository(ProjectDbContext dbContext) : base(dbContext)
        {
        }
    }
}