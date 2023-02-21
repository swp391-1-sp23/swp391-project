using SWP391.Project.DbContexts;
using SWP391.Project.Entities;

namespace SWP391.Project.Repositories
{
    public interface IAccountRepository : IBaseRepository<AccountEntity>
    {
        Task<AccountEntity?> GetAccountByEmailAsync(string email);
    }

    public class AccountRepository :
        BaseRepository<AccountEntity>, IAccountRepository
    {
        public AccountRepository(ProjectDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<AccountEntity?> GetAccountByEmailAsync(string email)
        {
            return await GetSingleAsync(predicate: item => item.Email == email);
        }

    }
}