using SWP391.Project.DbContexts;
using SWP391.Project.Entities;

namespace SWP391.Project.Repositories
{
    public interface ICartRepository : IBaseRepository<CartEntity>
    {
    }

    public class CartRepository : BaseRepository<CartEntity>, ICartRepository
    {
        public CartRepository(ProjectDbContext dbContext) : base(dbContext)
        {
        }
    }
}