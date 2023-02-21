using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using swp391-project.Project.DbContexts;
using swp391-project.Project.Entities;

namespace swp391-project.Repositories
{
    public interface IOrderRepository : IBaseRepository<OrderEntity>
    {
        Task<ICollection<IGrouping<Guid, OrderEntity>>> GetByAccountId(Guid accountId);
    }

    public class OrderRepository : BaseRepository<OrderEntity>, IOrderRepository
    {
        public OrderRepository(ProjectDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ICollection<IGrouping<Guid, OrderEntity>>> GetByAccountId(Guid accountId)
        {
            ICollection<OrderEntity> collection = await GetCollectionAsync(predicate: entity =>
                entity.ShipmentAddress != null
                && entity.ShipmentAddress.Account != null
                && entity.ShipmentAddress.Account.Id == accountId);

            List<IGrouping<Guid, OrderEntity>> result = collection.GroupBy(keySelector: entity => entity.Tag).ToList();

            return result;
        }
    }
}