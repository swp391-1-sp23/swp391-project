using Microsoft.EntityFrameworkCore;

using SWP391.Project.DbContexts;
using SWP391.Project.Entities;

namespace SWP391.Project.Repositories
{
    public interface IAccountRepository
    {
        Task<IEnumerable<AccountEntity>> GetAccounts();
        Task<bool> AddAccount(AccountEntity account);
        Task<AccountEntity?> GetAccountById(Guid accountId);
        Task<AccountEntity?> GetAccountByEmail(string accountEmail);
    }

    public class AccountRepository : IAccountRepository
    {
        private readonly ProjectDbContext _dbContext;

        public AccountRepository(ProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<AccountEntity>> GetAccounts()
        {
            return await _dbContext.Accounts.ToListAsync();
        }

        public async Task<bool> AddAccount(AccountEntity account)
        {
            try
            {
                _ = await _dbContext.Accounts.AddAsync(account);
                _ = await _dbContext.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<AccountEntity?> GetAccountById(Guid accountId)
        {
            return await _dbContext.Accounts.FirstOrDefaultAsync(item => item.Id == accountId);
        }

        public async Task<AccountEntity?> GetAccountByEmail(string accountEmail)
        {
            return await _dbContext.Accounts.FirstOrDefaultAsync(item => item.Email == accountEmail);
        }
    }
}