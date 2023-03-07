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
        Task<bool> AddCartProductAsync(AddCartProductDto input);
        Task<bool> UpdateCartProductQuantityAsync(UpdateCartProductQuantityDto input);
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

        public async Task<bool> AddCartProductAsync(AddCartProductDto input)
        {
            CartEntity? existingCartProduct = await _cartRepository
                .GetSingleAsync(predicate: cartProduct =>
                    cartProduct.Account != null
                    && cartProduct.Account.Id == input.AccountId
                    && cartProduct.Product != null
                    && cartProduct.Product.Id == input.ProductInStockId
                );

            if (existingCartProduct != null)
            {
                existingCartProduct.Quantity = input.Quantity;
                return await _cartRepository.UpdateAsync(entity: existingCartProduct);
            }

            CartEntity newCartProduct = new()
            {
                Account = await _accountRepository.GetByIdAsync(entityId: input.AccountId),
                Product = await _inStockRepository.GetByIdAsync(entityId: input.ProductInStockId),
                Quantity = input.Quantity
            };

            return await _cartRepository.AddAsync(entity: newCartProduct);
        }

        public async Task<ICollection<CartDto>> GetProductCollectionByAccountIdAsync(Guid accountId)
        {
            var products = await _cartRepository.GetCollectionAsync(predicate: product =>
                product.Account != null
                && product.Account.Id == accountId
            );

            return Mapper.Map<ICollection<CartDto>>(products);
        }

        public async Task<bool> UpdateCartProductQuantityAsync(UpdateCartProductQuantityDto input)
        {
            CartEntity? existingCartProduct = await _cartRepository.GetByIdAsync(entityId: input.CartProductId);

            if (existingCartProduct == null)
            {
                return false;
            }
existingCartProduct.Quantity = input.Quantity;

            return await _cartRepository.UpdateAsync(entity: existingCartProduct);
        }
    }
}
