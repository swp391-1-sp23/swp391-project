using SWP391.Project.Entities;
using SWP391.Project.Models.Dtos.Image;
using SWP391.Project.Models.Dtos.Product;
using SWP391.Project.Repositories;

namespace SWP391.Project.Services
{
    public interface IProductService
    {
        Task<ICollection<ProductEntity>> GetProductCollectionAsync(string? searchKey, string? color, string? size, string? brand);
        Task<bool> AddProductAsync(AddProductDto input);
        Task<bool> UpdateProductAsync(UpdateProductDto input);
        Task<bool> RemoveProductAsync(Guid productId);
        Task<bool> AddProductImagesAsync(AddProductImagesDto input);
        Task<bool> RemoveProductImageAsync(Guid imageId);
        Task<bool> AddProductColorsAsync(AddProductColorsDto input);
        Task<bool> RemoveProductColorAsync(Guid colorId);
        Task<bool> AddProductSizesAsync(AddProductSizesDto input);
        Task<bool> RemoveProductSizeAsync(Guid sizeId);
        Task<bool> AddProductQuantityAsync(AddProductQuantityDto input);
        Task<bool> UpdateProductQuantityAsync(UpdateProductQuantityDto input);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductInStockRepository _inStockRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IColorRepository _colorRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMinioRepository _minioRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IAccountRepository _accountRepository;

        public ProductService(IMinioRepository minioRepository,
                              IImageRepository imageRepository,
                              IColorRepository colorRepository,
                              ISizeRepository sizeRepository,
                              IProductInStockRepository inStockRepository,
                              IProductRepository productRepository,
                              IBrandRepository brandRepository,
                              IFeedbackRepository feedbackRepository,
                              IOrderRepository orderRepository,
                              IAccountRepository accountRepository)
        {
            _sizeRepository = sizeRepository;
            _minioRepository = minioRepository;
            _imageRepository = imageRepository;
            _colorRepository = colorRepository;
            _brandRepository = brandRepository;
            _inStockRepository = inStockRepository;
            _productRepository = productRepository;
            _feedbackRepository = feedbackRepository;
            _orderRepository = orderRepository;
            _accountRepository = accountRepository;
        }

        public Task<bool> AddProductAsync(AddProductDto input)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddProductColorsAsync(AddProductColorsDto input)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddProductImagesAsync(AddProductImagesDto input)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddProductQuantityAsync(AddProductQuantityDto input)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddProductSizesAsync(AddProductSizesDto input)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ProductEntity>> GetProductCollectionAsync(string? searchKey, string? color, string? size, string? brand)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveProductAsync(Guid productId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveProductColorAsync(Guid colorId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveProductImageAsync(Guid imageId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveProductSizeAsync(Guid sizeId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateProductAsync(UpdateProductDto input)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateProductQuantityAsync(UpdateProductQuantityDto input)
        {
            throw new NotImplementedException();
        }
    }
}