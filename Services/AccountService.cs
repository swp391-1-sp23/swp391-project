using SWP391.Project.Entities;
using SWP391.Project.Models.Dtos.Account;
using SWP391.Project.Repositories;

namespace SWP391.Project.Services
{
    public interface IAccountService
    {
        Task<ICollection<AccountEntity>> GetAccountCollectionAsync(string searchKey);
        Task<bool> RemoveCustomerAccountAsync(Guid accountId);
        Task<bool> UpdateAccountAsync(UpdateAccountDto input);
    }

    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMinioRepository _minioRepository;

        public AccountService(IAccountRepository accountRepository,
                              IImageRepository imageRepository,
                              IMinioRepository minioRepository)
        {
            _accountRepository = accountRepository;
            _imageRepository = imageRepository;
            _minioRepository = minioRepository;
        }

        public async Task<ICollection<AccountEntity>> GetAccountCollectionAsync(string searchKey)
        {
            return await _accountRepository
                .GetCollectionAsync(predicate: account =>
                    account.Id == Guid.Parse(input: searchKey)
                    || account.Email.Contains(value: searchKey)
                    || string.Join(
                            separator: ' ',
                            account.FirstName.Normalize(),
                            account.LastName.Normalize())
                        .Contains(value: searchKey)
                    );
        }

        public async Task<bool> RemoveCustomerAccountAsync(Guid accountId)
        {
            AccountEntity? account = await _accountRepository.GetByIdAsync(entityId: accountId);
            return account != null && await _accountRepository.RemoveAsync(account);
        }

        public async Task<bool> UpdateAccountAsync(UpdateAccountDto input)
        {
            AccountEntity? account = await _accountRepository.GetByIdAsync(entityId: input.AccountId);

            if (account == null)
            {
                return false;
            }

            account.FirstName = input.FirstName;
            account.LastName = input.LastName;
            account.Email = input.Email;
            account.Phone = input.Phone;

            return await _accountRepository.UpdateAsync(entity: account);
        }
    }
}