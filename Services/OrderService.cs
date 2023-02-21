using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SWP391.Project.Entities;
using SWP391.Project.Models.Dtos.Order;
using SWP391.Project.Repositories;

namespace SWP391.Project.Services
{
    public interface IOrderService
    {

        Task<bool> AddOrderAsync(AddOrderDto input);
        Task<bool> UpdateOrderStatusAsync(UpdateOrderStatusDto input);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IAddressRepository _addressRepository;

        public OrderService(IAddressRepository addressRepository,
                            ICartRepository cartRepository,
                            IOrderRepository orderRepository)
        {
            _addressRepository = addressRepository;
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
        }

        public async Task<bool> AddOrderAsync(AddOrderDto input)
        {
            ICollection<OrderEntity> newOrderCollection = new List<OrderEntity>();
            ICollection<CartEntity> currentCartProductCollection = new List<CartEntity>();

            DateTime orderDate = DateTime.Now;
            Guid orderTag = Guid.NewGuid();

            AddressEntity? shipmentAddress = await _addressRepository.GetByIdAsync(entityId: input.ShippingAddressId);

            foreach (Guid item in input.Products)
            {
                CartEntity? cartProduct = await _cartRepository.GetByIdAsync(entityId: item);

                if (cartProduct == null)
                {
                    continue;
                }

                currentCartProductCollection.Add(item: cartProduct);

                OrderEntity newOrder = new()
                {
                    ShipmentAddress = shipmentAddress,
                    Date = orderDate,
                    Tag = orderTag,
                    Product = cartProduct.Product,
                    Quantity = cartProduct.Quantity,
                };

                newOrderCollection.Add(item: newOrder);
            }

            bool orderCreateSuccess = await _orderRepository.AddCollectionAsync(collection: newOrderCollection);
            bool cartUpdateSuccess = await _cartRepository.RemoveCollectionAsync(collection: currentCartProductCollection);

            return orderCreateSuccess && cartUpdateSuccess;
        }

        public async Task<bool> UpdateOrderStatusAsync(UpdateOrderStatusDto input)
        {
            ICollection<OrderEntity> existingOrderCollection = await _orderRepository.GetCollectionAsync(predicate: order => order.Tag == input.Tag);

            foreach (OrderEntity item in existingOrderCollection)
            {
                item.Status = input.Status;
            }

            return await _orderRepository.UpdateCollectionAsync(collection: existingOrderCollection);
        }
    }
}