using AutoMapper;

using SWP391.Project.Entities;
using SWP391.Project.Models.Dtos.Brand;
using SWP391.Project.Models.Dtos.Color;
using SWP391.Project.Models.Dtos.Image;
using SWP391.Project.Models.Dtos.Product;
using SWP391.Project.Models.Dtos.Size;
using SWP391.Project.Repositories;

namespace SWP391.Project.Services
{
    public interface IProductService
    {
        Task<ICollection<ProductDto>> GetProductCollectionAsync(FilterProductDto? input = null);
        Task<ProductDto?> GetProductAsync(Guid productId);
        Task<bool> AddProductAsync(Guid brandId, AddProductDto input);
        Task<bool> UpdateProductAsync(Guid productId, UpdateProductDto input);
        Task<bool> RemoveProductAsync(Guid productId);
        Task<bool> AddProductImagesAsync(Guid productId, AddProductImagesDto input);
        Task<bool> RemoveProductImageAsync(Guid productId, Guid imageId);
        Task<bool> AddProductColorsAsync(Guid productId, AddProductColorsDto input);
        Task<bool> RemoveProductColorAsync(Guid productId, Guid colorId);
        Task<bool> AddProductSizesAsync(Guid productId, AddProductSizesDto input);
        Task<bool> RemoveProductSizeAsync(Guid productId, Guid sizeId);
        Task<bool> AddProductQuantityAsync(Guid productId, AddProductQuantityDto input);
        Task<bool> UpdateProductQuantityAsync(Guid productId, UpdateProductQuantityDto input);
        Task<ICollection<BrandDto>?> GetBrandCollectionAsync(FilterBrandDto? input = null);
        Task<ICollection<ColorDto>?> GetColorCollectionAsync(Guid productId);
        Task<ICollection<SizeDto>?> GetSizeCollectionAsync(Guid productId);
        Task<bool> AddBrandAsync(AddBrandDto input);
        Task<bool> RemoveBrandAsync(Guid brandId);
        Task<bool> UpdateBrandAsync(Guid brandId, UpdateBrandDto input);
        Task<bool> UpdateBrandLogoAsync(Guid brandId, UpdateBrandLogoDto input);
    }

    public class ProductService : BaseService, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductInStockRepository _inStockRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IColorRepository _colorRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IAccountRepository _accountRepository;

        public ProductService(IMinioRepository minioRepository,
                              IFileRepository fileRepository,
                              IColorRepository colorRepository,
                              ISizeRepository sizeRepository,
                              IProductInStockRepository inStockRepository,
                              IProductRepository productRepository,
                              IBrandRepository brandRepository,
                              IFeedbackRepository feedbackRepository,
                              IOrderRepository orderRepository,
                              IAccountRepository accountRepository,
                              IMapper mapper) : base(fileRepository, minioRepository, mapper)
        {
            _sizeRepository = sizeRepository;
            _fileRepository = fileRepository;
            _colorRepository = colorRepository;
            _brandRepository = brandRepository;
            _inStockRepository = inStockRepository;
            _productRepository = productRepository;
            _feedbackRepository = feedbackRepository;
            _orderRepository = orderRepository;
            _accountRepository = accountRepository;
        }

        public async Task<bool> AddBrandAsync(AddBrandDto input)
        {
            BrandEntity brand = Mapper.Map<BrandEntity>(input);

            return await _brandRepository.AddAsync(entity: brand);
        }

        public async Task<bool> AddProductAsync(Guid brandId, AddProductDto input)
        {
            ProductEntity product = Mapper.Map<ProductEntity>(input);

            product.Brand = await _brandRepository.GetByIdAsync(entityId: brandId);

            return await _productRepository.AddAsync(entity: product);
        }

        public async Task<bool> AddProductColorsAsync(Guid productId, AddProductColorsDto input)
        {
            ProductEntity? product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
            {
                return false;
            }

            ICollection<ColorEntity> newColorCollection = new List<ColorEntity>();

            foreach (string colorName in input.ColorNames)
            {
                ColorEntity color = new()
                {
                    ColorName = colorName,
                    Product = product
                };

                newColorCollection.Add(color);
            }

            return await _colorRepository.AddCollectionAsync(collection: newColorCollection);
        }

        public Task<bool> AddProductImagesAsync(Guid productId, AddProductImagesDto input)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddProductQuantityAsync(Guid productId, AddProductQuantityDto input)
        {
            ProductInStockEntity? existingInStock = await _inStockRepository.GetSingleAsync(predicate: item => item.Product!.Id == productId && item.Color!.Id == input.ColorId && item.Size!.Id == input.SizeId);

            if (existingInStock != null)
            {
                return false;
            }

            ProductInStockEntity newProductInStock = new()
            {
                Color = await _colorRepository.GetByIdAsync(entityId: input.ColorId),
                Size = await _sizeRepository.GetByIdAsync(entityId: input.SizeId),
                Product = await _productRepository.GetByIdAsync(entityId: productId),
                Quantity = input.Quantity
            };

            return await _inStockRepository.AddAsync(entity: newProductInStock);
        }

        public async Task<bool> AddProductSizesAsync(Guid productId, AddProductSizesDto input)
        {
            ProductEntity? product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
            {
                return false;
            }

            ICollection<SizeEntity> newSizeCollection = new List<SizeEntity>();

            foreach (string sizeName in input.SizeNames)
            {
                SizeEntity size = new() { SizeName = sizeName, Product = product };

                newSizeCollection.Add(size);
            }

            return await _sizeRepository.AddCollectionAsync(collection: newSizeCollection);
        }

        public async Task<ICollection<BrandDto>?> GetBrandCollectionAsync(FilterBrandDto? input = null)
        {
            ICollection<BrandEntity> brandCollection = await _brandRepository.GetCollectionAsync();

            ICollection<BrandDto> brandCollectionDto = Mapper.Map<ICollection<BrandDto>>(brandCollection).Select(selector: (item, idx) =>
            {
                BrandEntity brand = brandCollection.ElementAt(idx);

                (FileSimplified FileInfo, string? FileUrl)? logoFile = GetFileAsync(bucket: AvailableBucket.Brand, fileId: brand.Logo!.Id, fileExtension: brand.Logo.FileExtension).Result;

                if (logoFile != null)
                {
                    item.Logo = new()
                    {
                        LogoInfo = logoFile?.FileInfo!,
                        LogoUrl = logoFile?.FileUrl
                    };
                }

                return item;
            }).ToList();

            return brandCollectionDto;
        }

        public async Task<ICollection<ColorDto>?> GetColorCollectionAsync(Guid productId)
        {
            ICollection<ColorEntity> colorCollection = await _colorRepository.GetCollectionAsync(predicate: item => item.Product!.Id == productId);

            ICollection<ColorDto> colorCollectionDto = Mapper.Map<ICollection<ColorDto>>(colorCollection);

            return colorCollectionDto;
        }

        public async Task<ProductDto?> GetProductAsync(Guid productId)
        {
            ProductEntity? product = await _productRepository.GetByIdAsync(entityId: productId);

            if (product == null)
            {
                return null;
            }

            ProductDto productDto = await GetProductDtoAsync(product);

            return productDto;
        }



        public async Task<ICollection<ProductDto>> GetProductCollectionAsync(FilterProductDto? input = null)
        {
            ICollection<ProductEntity> productCollection = await _productRepository.GetCollectionAsync(predicate: item => !item.Brand!.IsDeleted);

            List<ProductDto> productCollectionDto = productCollection.Select(selector: item => GetProductDtoAsync(item).Result).ToList();

            return productCollectionDto;
        }

        public async Task<ICollection<SizeDto>?> GetSizeCollectionAsync(Guid productId)
        {
            ICollection<SizeEntity> sizeCollection = await _sizeRepository.GetCollectionAsync(predicate: item => item.Product!.Id == productId);

            ICollection<SizeDto> sizeCollectionDto = Mapper.Map<ICollection<SizeDto>>(sizeCollection);

            return sizeCollectionDto;
        }

        public async Task<bool> RemoveBrandAsync(Guid brandId)
        {
            BrandEntity? brand = await _brandRepository.GetByIdAsync(entityId: brandId);

            return brand != null && await _brandRepository.RemoveAsync(entity: brand);
        }

        public async Task<bool> RemoveProductAsync(Guid productId)
        {
            ProductEntity? product = await _productRepository.GetByIdAsync(entityId: productId);

            return product != null && await _productRepository.RemoveAsync(entity: product);
        }

        public async Task<bool> RemoveProductColorAsync(Guid productId, Guid colorId)
        {
            ColorEntity? color = await _colorRepository.GetByIdAsync(entityId: colorId);

            return color != null && await _colorRepository.RemoveAsync(entity: color);
        }

        public async Task<bool> RemoveProductImageAsync(Guid productId, Guid imageId)
        {
            FileEntity? image = await _fileRepository.GetByIdAsync(entityId: imageId);

            return image != null && await _fileRepository.RemoveAsync(entity: image);
        }

        public async Task<bool> RemoveProductSizeAsync(Guid productId, Guid sizeId)
        {
            SizeEntity? size = await _sizeRepository.GetByIdAsync(entityId: sizeId);

            return size != null && await _sizeRepository.RemoveAsync(entity: size);
        }

        public async Task<bool> UpdateBrandAsync(Guid brandId, UpdateBrandDto input)
        {
            BrandEntity? brand = await _brandRepository.GetByIdAsync(entityId: brandId);

            if (brand == null)
            {
                return false;
            }

            brand = Mapper.Map(source: input, destination: brand);

            return await _brandRepository.UpdateAsync(entity: brand);
        }

        public Task<bool> UpdateBrandLogoAsync(Guid brandId, UpdateBrandLogoDto input)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateProductAsync(Guid productId, UpdateProductDto input)
        {
            ProductEntity? product = await _productRepository.GetByIdAsync(entityId: productId);

            if (product == null)
            {
                return false;
            }

            product = Mapper.Map(source: input, destination: product);

            return await _productRepository.UpdateAsync(entity: product);
        }

        public async Task<bool> UpdateProductQuantityAsync(Guid productId, UpdateProductQuantityDto input)
        {
            ProductInStockEntity? productInStock = await _inStockRepository
                .GetSingleAsync(predicate: item =>
                    item.Id == input.ProductInStockId
                    && item.Color!.Id == input.ColorId && item.Size!.Id == input.SizeId);

            if (productInStock == null)
            {
                return false;
            }

            productInStock.Quantity += input.Quantity;

            bool success = await _inStockRepository.UpdateAsync(entity: productInStock);

            return success;
        }

        public async Task<ProductDto> GetProductDtoAsync(ProductEntity product)
        {
            ProductDto productDto = Mapper.Map<ProductDto>(product);

            productDto.Images = product.Images?.Select(selector: (item, idx) =>
            {
                (FileSimplified FileInfo, string? FileUrl)? imageFile = GetFileAsync(bucket: AvailableBucket.Product, fileId: item.Id, fileExtension: item.FileExtension).Result;

                return (imageFile?.FileInfo!, imageFile?.FileUrl!);
            }).ToList();

            productDto.Brand = Mapper.Map<BrandSimplified>(product.Brand);

            ICollection<ColorEntity> productColors = await _colorRepository
                .GetCollectionAsync(predicate: item => item.Product?.Id == productDto.Id);

            if (productColors != null)
            {
                productDto.Colors = Mapper.Map<ICollection<ColorSimplified>>(productColors);
            }

            ICollection<SizeEntity> productSizes = await _sizeRepository
                .GetCollectionAsync(predicate: item => item.Product?.Id == productDto.Id);

            if (productSizes != null)
            {
                productDto.Sizes = Mapper.Map<ICollection<SizeSimplified>>(productSizes);
            }

            if (productDto.Colors != null && productDto.Sizes != null)
            {
                ICollection<ProductInStockEntity> inStocks =
                    await _inStockRepository
                        .GetCollectionAsync(predicate: item =>
                            item.Product!.Id == productDto.Id
                            && productDto.Colors.Any(color => color.Id == item.Color?.Id)
                            && productDto.Sizes.Any(size => size.Id == item.Size?.Id));

                productDto.InStocks = inStocks.ToDictionary(keySelector: item => item.Id, elementSelector: item => (item.Color!.Id, item.Size!.Id, item.Quantity));
            }

            return productDto;
        }
    }
}
