using AutoMapper;

using SWP391.Project.Entities;
using SWP391.Project.Models.Dtos.Account;
using SWP391.Project.Models.Dtos.Image;
using SWP391.Project.Repositories;

namespace SWP391.Project.Services
{
    public interface IAccountService
    {
        Task<ICollection<AccountDto>> GetAccountCollectionAsync(FilterAccountDto filter);
        Task<bool> RemoveCustomerAccountAsync(Guid accountId);
        Task<bool> RemoveShopAccountAsync(Guid accountId);
        Task<bool> UpdateAccountAsync(UpdateAccountDto input);
        Task<bool> UpdateAccountAvatarAsync(UpdateAccountAvatarDto input);
        Task<AccountDto> GetAccountAsync(Guid accountId);
    }

    public class AccountService : BaseService, IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMinioRepository _minioRepository;

        public AccountService(IAccountRepository accountRepository,
                              IImageRepository imageRepository,
                              IMinioRepository minioRepository,
                              IMapper mapper) : base(mapper)
        {
            _accountRepository = accountRepository;
            _imageRepository = imageRepository;
            _minioRepository = minioRepository;
        }

        public async Task<AccountDto> GetAccountAsync(Guid accountId)
        {
            var account = await _accountRepository.GetByIdAsync(accountId);
            var accountDto = Mapper.Map<AccountDto>(account);
            return accountDto;
        }

        public async Task<ICollection<AccountDto>> GetAccountCollectionAsync(FilterAccountDto filter)
        {
            var searchKey = filter.SearchKey;
            var accountCollection = await _accountRepository
                .GetCollectionAsync(predicate: account =>
                    account.Id == Guid.Parse(input: searchKey)
                    || account.Email == searchKey
                    || string.Join(
                            separator: ' ',
                            account.FirstName.Normalize(),
                            account.LastName.Normalize())
                        .Contains(value: searchKey.Normalize())
                    );

            var accountDtos = Mapper.Map<ICollection<AccountDto>>(accountCollection);
            return accountDtos;
        }

        public async Task<bool> RemoveCustomerAccountAsync(Guid accountId)
        {
            return await RemoveAccountAsync(accountId);
        }

        public async Task<bool> RemoveShopAccountAsync(Guid accountId)
        {
            return await RemoveAccountAsync(accountId);
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

        public Task<bool> UpdateAccountAvatarAsync(UpdateAccountAvatarDto input)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> RemoveAccountAsync(Guid accountId)
        {
            AccountEntity? account = await _accountRepository.GetByIdAsync(entityId: accountId);

            return account == null || await _accountRepository.RemoveAsync(account);
        }
    }
}