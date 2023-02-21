using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SWP391.Project.DbContexts;
using SWP391.Project.Entities;

namespace SWP391.Project.Repositories
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