using Microsoft.AspNetCore.Mvc;

using SWP391.Project.Models.Dtos.Brand;
using SWP391.Project.Models.Dtos.Image;
using SWP391.Project.Models.Dtos.Product;
using SWP391.Project.Services;

namespace SWP391.Project.Controllers
{
    public interface IProductController
    {
        Task<ActionResult> GetProductAsync([FromRoute] Guid productId);
        Task<ActionResult> GetProductCollectionAsync([FromQuery] FilterProductDto input);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult> AddProductAsync([FromBody] AddProductDto input);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult> UpdateProductAsync([FromBody] UpdateProductDto input);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult> RemoveProductAsync([FromRoute] Guid productId);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult> AddProductSizesAsync([FromBody] AddProductSizesDto input);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult> RemoveProductSizeAsync([FromRoute] Guid sizeId);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult> AddProductColorsAsync([FromBody] AddProductColorsDto input);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult> RemoveProductColorsAsync([FromRoute] Guid colorId);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult> AddProductQuantityAsync([FromBody] AddProductQuantityDto input);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult> UpdateProductQuantityAsync([FromBody] UpdateProductQuantityDto input);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult> AddProductImagesAsync([FromForm] AddProductImagesDto input);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult> RemoveProductImageAsync([FromRoute] Guid imageId);
        Task<ActionResult> GetBrandAsync([FromRoute] Guid brandId);
        Task<ActionResult> GetBrandCollectionAsync([FromQuery] FilterBrandDto input);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult> AddBrandAsync([FromBody] AddBrandDto input);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult> UpdateBrandAsync([FromBody] UpdateBrandDto input);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult> UpdateBrandLogoAsync([FromForm] UpdateBrandLogoDto input);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult> RemoveBrandAsync([FromRoute] Guid brandId);
    }

    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase, IProductController
    {
        private readonly IProductService _productService;
        private readonly IFeedbackService _feedbackService;

        public ProductController(IFeedbackService feedbackService,
                                 IProductService productService)
        {
            _feedbackService = feedbackService;
            _productService = productService;
        }

        public Task<ActionResult> AddBrandAsync([FromBody] AddBrandDto input)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> AddProductAsync([FromBody] AddProductDto input)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> AddProductColorsAsync([FromBody] AddProductColorsDto input)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> AddProductImagesAsync([FromForm] AddProductImagesDto input)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> AddProductQuantityAsync([FromBody] AddProductQuantityDto input)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> AddProductSizesAsync([FromBody] AddProductSizesDto input)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> GetBrandAsync([FromRoute] Guid brandId)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> GetBrandCollectionAsync([FromQuery] FilterBrandDto input)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> GetProductAsync([FromRoute] Guid productId)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> GetProductCollectionAsync([FromQuery] FilterProductDto input)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> RemoveBrandAsync([FromRoute] Guid brandId)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> RemoveProductAsync([FromRoute] Guid productId)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> RemoveProductColorsAsync([FromRoute] Guid colorId)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> RemoveProductImageAsync([FromRoute] Guid imageId)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> RemoveProductSizeAsync([FromRoute] Guid sizeId)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> UpdateBrandAsync([FromBody] UpdateBrandDto input)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> UpdateBrandLogoAsync([FromForm] UpdateBrandLogoDto input)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> UpdateProductAsync([FromBody] UpdateProductDto input)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> UpdateProductQuantityAsync([FromBody] UpdateProductQuantityDto input)
        {
            throw new NotImplementedException();
        }
    }
}