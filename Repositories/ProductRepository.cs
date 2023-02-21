using SWP391.Project.DbContexts;
using SWP391.Project.Entities;

namespace SWP391.Project.Repositories
{
    public interface IProductRepository : IBaseRepository<ProductEntity>
    {
    }

    public class ProductRepository : BaseRepository<ProductEntity>, IProductRepository
    {
        public ProductRepository(ProjectDbContext dbContext) : base(dbContext)
        {
        }
    }
}