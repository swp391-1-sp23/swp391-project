using AutoMapper;

using SWP391.Project.Entities;
using SWP391.Project.Models.Dtos.Account;
using SWP391.Project.Models.Dtos.Image;
using SWP391.Project.Repositories;

namespace SWP391.Project.Services
{
    public interface IAccountService
    {
        Task<ICollection<AccountDto>> GetAccountCollectionAsync(FilterAccountDto? filter = null);
        Task<bool> RemoveCustomerAccountAsync(Guid accountId);
        Task<bool> RemoveShopAccountAsync(Guid accountId);
        Task<bool> UpdateAccountAsync(Guid accountId, UpdateAccountDto input);
        Task<bool> UpdateAccountAvatarAsync(Guid accountId, UpdateAccountAvatarDto input);
        Task<AccountDto?> GetAccountAsync(Guid accountId);
    }

    public class AccountService : BaseService, IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository,
                              IFileRepository fileRepository,
                              IMinioRepository minioRepository,
                              IMapper mapper) : base(fileRepository,
                                                     minioRepository,
                                                     mapper)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountDto?> GetAccountAsync(Guid accountId)
        {
            AccountEntity? account = await _accountRepository.GetByIdAsync(accountId);

            if (account == null)
            {
                return null;
            }

            AccountDto accountDto = Mapper.Map<AccountDto>(account);

            (FileSimplified FileInfo, string? FileUrl)? avatarFile = await GetFileAsync(bucket: AvailableBucket.Avatar, fileId: account.Avatar!.Id, fileExtension: account.Avatar!.FileExtension);

            if (avatarFile != null)
            {
                accountDto.Avatar = new()
                {
                    AvatarInfo = avatarFile?.FileInfo!,
                    AvatarUrl = avatarFile?.FileUrl
                };
            }

            return accountDto;
        }

        public async Task<ICollection<AccountDto>> GetAccountCollectionAsync(FilterAccountDto? filter = null)
        {
            string searchKey = filter?.SearchKey ?? string.Empty;

            ICollection<AccountEntity> accountCollection = await _accountRepository
                .GetCollectionAsync(predicate: account =>
                    account.Id.ToString() == searchKey
                    || account.Email == searchKey
                    || string.Join(
                            separator: ' ',
                            account.FirstName.Normalize(),
                            account.LastName.Normalize())
                        .Contains(value: searchKey.Normalize())
                    );

            ICollection<AccountDto> accountCollectionDto = Mapper.Map<ICollection<AccountDto>>(accountCollection).Select(selector: (item, idx) =>
            {
                AccountEntity account = accountCollection.ElementAt(idx);

                (FileSimplified FileInfo, string? FileUrl)? avatarFile = GetFileAsync(bucket: AvailableBucket.Avatar, fileId: account.Avatar!.Id, fileExtension: account.Avatar!.FileExtension).Result;

                if (avatarFile != null)
                {
                    item.Avatar = new()
                    {
                        AvatarInfo = avatarFile?.FileInfo!,
                        AvatarUrl = avatarFile?.FileUrl
                    };
                }

                return item;
            }).ToList();

            return accountCollectionDto;
        }

        public async Task<bool> RemoveCustomerAccountAsync(Guid accountId)
        {
            return await RemoveAccountAsync(accountId);
        }

        public async Task<bool> RemoveShopAccountAsync(Guid accountId)
        {
            return await RemoveAccountAsync(accountId);
        }

        public async Task<bool> UpdateAccountAsync(Guid accountId, UpdateAccountDto input)
        {
            AccountEntity? account = await _accountRepository.GetByIdAsync(entityId: accountId);

            if (account == null)
            {
                return false;
            }

            account = Mapper.Map(source: input, destination: account);

            return await _accountRepository.UpdateAsync(entity: account);
        }

        public Task<bool> UpdateAccountAvatarAsync(Guid accountId, UpdateAccountAvatarDto input)
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