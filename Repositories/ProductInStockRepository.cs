using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using swp391-project.Project.DbContexts;
using swp391-project.Project.Entities;

namespace swp391-project.Repositories
{
    public interface IProductInStockRepository : IBaseRepository<ProductInStockEntity>
    {
    }

    public class ProductInStockRepository : BaseRepository<ProductInStockEntity>, IProductInStockRepository
    {
        public ProductInStockRepository(ProjectDbContext dbContext) : base(dbContext)
        {
        }
    }
}