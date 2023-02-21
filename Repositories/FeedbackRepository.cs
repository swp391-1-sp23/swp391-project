using SWP391.Project.DbContexts;
using SWP391.Project.Entities;

namespace SWP391.Project.Repositories
{
    public interface IFeedbackRepository : IBaseRepository<FeedbackEntity>
    {
    }
    public class FeedbackRepository : BaseRepository<FeedbackEntity>, IFeedbackRepository
    {
        public FeedbackRepository(ProjectDbContext dbContext) : base(dbContext)
        {
        }
    }
}