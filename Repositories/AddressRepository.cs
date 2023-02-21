using SWP391.Project.DbContexts;
using SWP391.Project.Entities;

namespace SWP391.Project.Repositories
{
    public interface IAddressRepository : IBaseRepository<AddressEntity>
    {
        Task<ICollection<AddressEntity>> GetByAccountId(Guid accountId);
    }

    public class AddressRepository : BaseRepository<AddressEntity>, IAddressRepository
    {
        public AddressRepository(ProjectDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ICollection<AddressEntity>> GetByAccountId(Guid accountId)
        {
            ICollection<AddressEntity> result = await GetCollectionAsync(predicate: entity =>
                entity.Account != null
                && entity.Account.Id == accountId);
            return result;
        }
    }
}