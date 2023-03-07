using SWP391.Project.Models.Dtos.Address;

namespace SWP391.Project.Services
{
    public interface IShipmentService
    {
        Task<AddressDto> GetShipmentAddressAsync(Guid addressId);
        Task<ICollection<AddressDto>> GetShipmentAddressCollectionAsync(Guid accountId);
        Task<bool> AddShipmentAddressAsync(AddAddressDto input);
        Task<bool> RemoveShipmentAddressAsync(Guid addressId);
    }

    public class ShipmentService : IShipmentService
    {
        public Task<bool> AddShipmentAddressAsync(AddAddressDto input)
        {
            throw new NotImplementedException();
        }

        public Task<AddressDto> GetShipmentAddressAsync(Guid addressId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<AddressDto>> GetShipmentAddressCollectionAsync(Guid accountId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveShipmentAddressAsync(Guid addressId)
        {
            throw new NotImplementedException();
        }
    }
}
