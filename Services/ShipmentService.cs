using AutoMapper;
 
using SWP391.Project.Entities;
using SWP391.Project.Models.Dtos.Address;
using SWP391.Project.Repositories;
 
namespace SWP391.Project.Services
{
    public interface IShipmentService
    {
        Task<AddressDto> GetShipmentAddressAsync(Guid addressId);
        Task<ICollection<AddressDto>> GetShipmentAddressCollectionAsync(Guid accountId);
        Task<bool> AddShipmentAddressAsync(Guid accountId, AddAddressDto input);
        Task<bool> RemoveShipmentAddressAsync(Guid accountId, Guid addressId);
    }
 
    public class ShipmentService : BaseService, IShipmentService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IAccountRepository _accountRepository;
 
        public ShipmentService(IAddressRepository addressRepository,
                               IAccountRepository accountRepository,
                               IMapper mapper) : base(mapper)
        {
            _addressRepository = addressRepository;
            _accountRepository = accountRepository;
        }
 
        public async Task<bool> AddShipmentAddressAsync(Guid accountId, AddAddressDto input)
        {
            AddressEntity newAddress = Mapper.Map<AddressEntity>(input);
 
            newAddress.Account = await _accountRepository.GetByIdAsync(entityId: accountId);
 
            ICollection<AddressEntity> existingAddresses = await _addressRepository.GetCollectionAsync(predicate: address => address.Account!.Id == accountId);
 
            return !existingAddresses.Any(address => address.IsPrimary && newAddress.IsPrimary)
                   && await _addressRepository.AddAsync(entity: newAddress);
        }
 
        public async Task<AddressDto> GetShipmentAddressAsync(Guid addressId)
        {
            AddressEntity? existingAddress = await _addressRepository.GetByIdAsync(entityId: addressId);
 
            return Mapper.Map<AddressDto>(existingAddress);
        }
 
        public async Task<ICollection<AddressDto>> GetShipmentAddressCollectionAsync(Guid accountId)
        {
            ICollection<AddressEntity> addressCollection = await _addressRepository.GetCollectionAsync(predicate: address => address.Account!.Id == accountId);
 
            List<AddressDto> addressCollectionDto = Mapper.Map<ICollection<AddressDto>>(addressCollection).Select(selector: (item, idx) =>
            {
                item.Account = Mapper.Map<AccountSimplified>(addressCollection.ElementAt(idx).Account);
 
                return item;
            }).ToList();
 
            return addressCollectionDto;
        }
 
        public async Task<bool> RemoveShipmentAddressAsync(Guid accountId, Guid addressId)
        {
            AddressEntity? existingAddress = await _addressRepository.GetByIdAsync(entityId: addressId);
 
            return existingAddress != null && await _addressRepository.RemoveAsync(entity: existingAddress);
        }
    }
}
