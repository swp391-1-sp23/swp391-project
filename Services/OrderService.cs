using AutoMapper;
 
using SWP391.Project.Entities;
using SWP391.Project.Models.Dtos.Order;
using SWP391.Project.Repositories;

namespace SWP391.Project.Services
{
    public interface IOrderService
    {
        Task<bool> AddOrderAsync(Guid accountId, AddOrderDto input);
        Task<ICollection<OrderDto>> GetAccountOrderAsync(Guid accountId, Guid orderTag);
        Task<IDictionary<Guid, List<OrderDto>>> GetAccountOrderCollectionAsync(Guid accountId);
        Task<IDictionary<Guid, List<OrderDto>>> GetOrderCollectionAsync(FilterOrderDto? input = null);
        Task<bool> UpdateOrderStatusAsync(Guid accountId, Guid orderTag, UpdateOrderStatusDto input);
    }
 
    public class OrderService : BaseService, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IProductInStockRepository _inStockRepository;
 
        public OrderService(IAddressRepository addressRepository,
                            ICartRepository cartRepository,
                            IOrderRepository orderRepository,
                            IProductInStockRepository inStockRepository,
                            IMapper mapper) : base(mapper)
        {
            _addressRepository = addressRepository;
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _inStockRepository = inStockRepository;
        }
 
        public async Task<bool> AddOrderAsync(Guid accountId, AddOrderDto input)
        {
            ICollection<OrderEntity> orderCollection = new List<OrderEntity>();
            ICollection<CartEntity> cartProductCollection = new List<CartEntity>();
            ICollection<ProductInStockEntity> productInStockCollection = new List<ProductInStockEntity>();
 
            DateTime orderDate = DateTime.Now;
            Guid orderTag = Guid.NewGuid();
 
            AddressEntity? shipmentAddress = await _addressRepository.GetByIdAsync(entityId: input.ShippingAddressId);
 
            foreach (Guid item in input.ProductInStocks)
            {
                CartEntity? cartProduct = await _cartRepository.GetByIdAsync(entityId: item);
 
                if (cartProduct == null)
                {
                    continue;
                }
 
                cartProductCollection.Add(item: cartProduct);
 
                ProductInStockEntity? productInStock = cartProduct.Product;
 
                if (productInStock == null)
                {
                    continue;
                }
 
                productInStock.Quantity -= cartProduct.Quantity;
                productInStockCollection.Add(item: productInStock);
 
                OrderEntity newOrder = new()
                {
                    ShipmentAddress = shipmentAddress,
                    Date = orderDate,
                    Tag = orderTag,
                    Product = cartProduct.Product,
                    Quantity = cartProduct.Quantity,
                };
 
                orderCollection.Add(item: newOrder);
 
            }
 
            bool success = await _orderRepository.AddCollectionAsync(collection: orderCollection)
                           && await _cartRepository.RemoveCollectionAsync(collection: cartProductCollection)
                           && await _inStockRepository.UpdateCollectionAsync(collection: productInStockCollection);
 
            return success;
        }
 
        public async Task<ICollection<OrderDto>> GetAccountOrderAsync(Guid accountId, Guid orderTag)
        {
            ICollection<OrderEntity> orderCollection = await _orderRepository.GetCollectionAsync(predicate: item => item.ShipmentAddress!.Account!.Id == accountId && item.Tag == orderTag);
 
            ICollection<OrderDto> orderCollectionDto = Mapper
                .Map<ICollection<OrderDto>>(orderCollection)
                .Select(selector: (orderDto, idx) =>
                    MapOrderDtoDetails(orderDto, idx, orderCollection.ElementAt(idx)))
                .ToList();
 
            return orderCollectionDto;
        }
 
        public async Task<IDictionary<Guid, List<OrderDto>>> GetAccountOrderCollectionAsync(Guid accountId)
        {
            ICollection<OrderEntity> orderCollection = await _orderRepository.GetCollectionAsync(predicate: item => item.ShipmentAddress!.Account!.Id == accountId);
 
            Dictionary<Guid, List<OrderDto>> orderCollectionDto = Mapper
                .Map<ICollection<OrderDto>>(orderCollection)
                .Select(selector: (orderDto, idx) =>
                    MapOrderDtoDetails(orderDto, idx, orderCollection.ElementAt(idx)))
                .GroupBy(keySelector: item => item.Tag)
                .ToDictionary(keySelector: item => item.Key, elementSelector: item => item.ToList());
            return orderCollectionDto;
        }
 
        public Task<IDictionary<Guid, List<OrderDto>>> GetOrderCollectionAsync(FilterOrderDto? input = null)
        {
            throw new NotImplementedException();
        }
 
        public async Task<bool> UpdateOrderStatusAsync(Guid accountId, Guid orderTag, UpdateOrderStatusDto input)
        {
            ICollection<OrderEntity> existingOrderCollection = await _orderRepository.GetCollectionAsync(predicate: order => order.Tag == input.Tag);
 
            foreach (OrderEntity item in existingOrderCollection)
            {
                item.Status = input.Status;
            }
 
            return await _orderRepository.UpdateCollectionAsync(collection: existingOrderCollection);
        }
 
        private OrderDto MapOrderDtoDetails(OrderDto orderDto, int idx, OrderEntity order)
        {
            orderDto.Product = Mapper.Map<ProductSimplified>(order.Product!.Product);
            orderDto.Color = Mapper.Map<ColorSimplified>(order.Product!.Color);
            orderDto.Size = Mapper.Map<SizeSimplified>(order.Product!.Size);
            return orderDto;
        }
    }
}
