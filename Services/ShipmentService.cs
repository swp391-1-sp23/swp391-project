using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SWP391.Project.Models.Dtos.Address;

namespace SWP391.Project.Services
{
    public interface IShipmentService
    {

        Task<bool> AddShipmentAddressAsync(AddAddressDto input);
        Task<bool> RemoveShipmentAddressAsync(Guid addressId);
    }

    public class ShipmentService : IShipmentService
    {
        public Task<bool> AddShipmentAddressAsync(AddAddressDto input)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveShipmentAddressAsync(Guid addressId)
        {
            throw new NotImplementedException();
        }
    }
}