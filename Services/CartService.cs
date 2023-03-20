using AutoMapper;

using SWP391.Project.Entities;
using SWP391.Project.Models.Dtos.Cart;
using SWP391.Project.Models.Dtos.Product;
using SWP391.Project.Repositories;

namespace SWP391.Project.Services
{
    public interface ICartService
    {
        Task<ICollection<CartDto>> GetProductCollectionByAccountIdAsync(Guid accountId);
        Task<bool> AddCartProductAsync(Guid accountId, AddCartProductDto input);
        Task<bool> UpdateCartProductQuantityAsync(Guid accountId, UpdateCartProductQuantityDto input);
    }

    public class CartService : BaseService, ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductInStockRepository _inStockRepository;
        private readonly IAccountRepository _accountRepository;

        public CartService(IProductInStockRepository inStockRepository,
                           ICartRepository cartRepository,
                           IAccountRepository accountRepository,
                           IMapper mapper) : base(mapper)
        {
            _inStockRepository = inStockRepository;
            _cartRepository = cartRepository;
            _accountRepository = accountRepository;
        }

        public async Task<bool> AddCartProductAsync(Guid accountId, AddCartProductDto input)
        {
            CartEntity? cartProduct = await _cartRepository
                .GetSingleAsync(predicate: item =>
                    item.Account!.Id == accountId
                    && item.Product!.Id == input.ProductInStockId
                );

            if (cartProduct != null)
            {
                cartProduct.Quantity = input.Quantity;
                return await _cartRepository.UpdateAsync(entity: cartProduct);
            }

            cartProduct = new()
            {
                Account = await _accountRepository.GetByIdAsync(entityId: accountId),
                Product = await _inStockRepository.GetByIdAsync(entityId: input.ProductInStockId),
                Quantity = input.Quantity,
            };

            return await _cartRepository.AddAsync(entity: cartProduct);
        }

        public async Task<ICollection<CartDto>> GetProductCollectionByAccountIdAsync(Guid accountId)
        {
            ICollection<CartEntity> products = await _cartRepository.GetCollectionAsync(predicate: product => product.Account!.Id == accountId
            );

            ICollection<CartDto> productsDto = Mapper.Map<ICollection<CartDto>>(products);

            productsDto = productsDto.Select(selector: (item, idx) =>
            {
                item.Size = Mapper.Map<SizeSimplified>(products.ElementAt(idx).Product?.Size);
                item.Product = Mapper.Map<ProductSimplified>(products.ElementAt(idx).Product?.Product);
                item.Color = Mapper.Map<ColorSimplified>(products.ElementAt(idx).Product?.Color);

                return item;
            }).ToList();

            return productsDto;
        }

        public async Task<bool> UpdateCartProductQuantityAsync(Guid accountId, UpdateCartProductQuantityDto input)
        {
            CartEntity? cartProduct = await _cartRepository.GetByIdAsync(entityId: input.CartProductId);

            if (cartProduct == null)
            {
                return false;
            }

            if (input.Quantity == 0)
            {
                return await RemoveCartProductAsync(input.CartProductId);
            }

            cartProduct.Quantity = input.Quantity;

            return await _cartRepository.UpdateAsync(entity: cartProduct);
        }

        private async Task<bool> RemoveCartProductAsync(Guid cartId)
        {
            CartEntity? existingCartProduct = await _cartRepository.GetByIdAsync(entityId: cartId);

            return existingCartProduct != null && await _cartRepository.RemoveAsync(entity: existingCartProduct);
        }
    }
}
