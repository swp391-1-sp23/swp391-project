using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 
using SWP391.Project.Models;
using SWP391.Project.Models.Dtos.Brand;
using SWP391.Project.Models.Dtos.Image;
using SWP391.Project.Models.Dtos.Product;
using SWP391.Project.Services;
 
namespace SWP391.Project.Controllers
{
    public interface IProductController
    {
        Task<ActionResult<ResponseModel<ProductDto>>> GetProductAsync([FromRoute] Guid productId);
        Task<ActionResult<ResponseModel<ICollection<ProductDto>>>> GetProductCollectionAsync([FromQuery] FilterProductDto? input = null);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult<ResponseModel<bool>>> AddProductAsync([FromBody][FromRoute] Guid brandId, AddProductDto input);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult<ResponseModel<bool>>> UpdateProductAsync([FromRoute] Guid productId, [FromBody] UpdateProductDto input);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult<ResponseModel<bool>>> RemoveProductAsync([FromRoute] Guid productId);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult<ResponseModel<bool>>> AddProductSizesAsync([FromRoute] Guid productId, [FromBody] AddProductSizesDto input);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult<ResponseModel<bool>>> RemoveProductSizeAsync([FromRoute] Guid productId, [FromRoute] Guid sizeId);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult<ResponseModel<bool>>> AddProductColorsAsync([FromRoute] Guid productId, [FromBody] AddProductColorsDto input);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult<ResponseModel<bool>>> RemoveProductColorAsync([FromRoute] Guid productId, [FromRoute] Guid colorId);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult<ResponseModel<bool>>> AddProductQuantityAsync([FromRoute] Guid productId, [FromBody] AddProductQuantityDto input);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult<ResponseModel<bool>>> UpdateProductQuantityAsync([FromRoute] Guid productId, [FromBody] UpdateProductQuantityDto input);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult<ResponseModel<bool>>> AddProductImagesAsync([FromRoute] Guid productId, [FromForm] AddProductImagesDto input);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult<ResponseModel<bool>>> RemoveProductImageAsync([FromRoute] Guid productId, [FromRoute] Guid imageId);
        Task<ActionResult<ResponseModel<ICollection<BrandDto>>>> GetBrandCollectionAsync([FromQuery] FilterBrandDto? input = null);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult<ResponseModel<bool>>> AddBrandAsync([FromBody] AddBrandDto input);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult<ResponseModel<bool>>> UpdateBrandAsync([FromRoute] Guid brandId, [FromBody] UpdateBrandDto input);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult<ResponseModel<bool>>> UpdateBrandLogoAsync([FromRoute] Guid brandId, [FromForm] UpdateBrandLogoDto input);
        // [Authorize(Roles = "Shop")]
        Task<ActionResult<ResponseModel<bool>>> RemoveBrandAsync([FromRoute] Guid brandId);
        // Task<ActionResult<ResponseModel<ICollection<SizeDto>>>> GetSizeCollectionAsync([FromRoute] Guid productId);
        // Task<ActionResult<ResponseModel<ICollection<ColorDto>>>> GetColorCollectionAsync([FromRoute] Guid productId);
    }
 
    [ApiController]
    [Route("api")]
    public class ProductController : BaseController, IProductController
    {
        private readonly IProductService _productService;
        private readonly IFeedbackService _feedbackService;
 
        public ProductController(IFeedbackService feedbackService,
                                 IProductService productService)
        {
            _feedbackService = feedbackService;
            _productService = productService;
        }
 
        [HttpPost(template: "brand")]
        [Authorize(Roles = "Shop")]
        public async Task<ActionResult<ResponseModel<bool>>> AddBrandAsync([FromBody] AddBrandDto input)
        {
            ResponseModel<bool> response = new();
 
            bool success = await _productService.AddBrandAsync(input);
 
            response.Success = success;
 
            if (!response.Success)
            {
                response.ErrorCode = "BRAND.CREATE.ERROR";
                return BadRequest(response);
            }
 
            return Ok(response);
        }
 
        [HttpPost(template: "brand/{brandId}/product")]
        [Authorize(Roles = "Shop")]
        public async Task<ActionResult<ResponseModel<bool>>> AddProductAsync([FromRoute] Guid brandId, [FromBody] AddProductDto input)
        {
            ResponseModel<bool> response = new();
 
            bool success = await _productService.AddProductAsync(brandId, input);
 
            response.Success = success;
 
            if (!response.Success)
            {
                response.ErrorCode = "PRODUCT.CREATE.ERROR";
                return BadRequest(response);
            }
 
            return Ok(response);
        }
 
        [HttpPost(template: "product/{productId}/color")]
        [Authorize(Roles = "Shop")]
        public async Task<ActionResult<ResponseModel<bool>>> AddProductColorsAsync([FromRoute] Guid productId, [FromBody] AddProductColorsDto input)
        {
            ResponseModel<bool> response = new();
 
            bool success = await _productService.AddProductColorsAsync(productId, input);
 
            response.Success = success;
 
            if (!response.Success)
            {
                response.ErrorCode = "PRODUCT_COLOR.CREATE.ERROR";
                return BadRequest(response);
            }
 
            return Ok(response);
        }
 
        [HttpPost(template: "product/{productId}/image")]
        [Authorize(Roles = "Shop")]
        public async Task<ActionResult<ResponseModel<bool>>> AddProductImagesAsync([FromRoute] Guid productId, [FromForm] AddProductImagesDto input)
        {
            ResponseModel<bool> response = new();
 
            bool success = await _productService.AddProductImagesAsync(productId, input);
 
            response.Success = success;
 
            if (!response.Success)
            {
                response.ErrorCode = "PRODUCT_IMAGE.CREATE.ERROR";
                return BadRequest(response);
            }
 
            return Ok(response);
        }
 
        [HttpPost(template: "product/{productId}/quantity")]
        [Authorize(Roles = "Shop")]
        public async Task<ActionResult<ResponseModel<bool>>> AddProductQuantityAsync([FromRoute] Guid productId, [FromBody] AddProductQuantityDto input)
        {
            ResponseModel<bool> response = new();
 
            bool success = await _productService.AddProductQuantityAsync(productId, input);
 
            response.Success = success;
 
            if (!response.Success)
            {
                response.ErrorCode = "PRODUCT_QUANTITY.CREATE.ERROR";
                return BadRequest(response);
            }
 
            return Ok(response);
        }
 
        [HttpPost(template: "product/{productId}/size")]
        [Authorize(Roles = "Shop")]
        public async Task<ActionResult<ResponseModel<bool>>> AddProductSizesAsync([FromRoute] Guid productId, [FromBody] AddProductSizesDto input)
        {
            ResponseModel<bool> response = new();
 
            bool success = await _productService.AddProductSizesAsync(productId, input);
 
            response.Success = success;
 
            if (!response.Success)
            {
                response.ErrorCode = "PRODUCT_SIZE.CREATE.ERROR";
                return BadRequest(response);
            }
 
            return Ok(response);
        }
 
        [HttpGet(template: "brand")]
        [Authorize(Roles = "Shop")]
        public async Task<ActionResult<ResponseModel<ICollection<BrandDto>>>> GetBrandCollectionAsync([FromQuery] FilterBrandDto? input = null)
        {
            ResponseModel<ICollection<BrandDto>> response = new();
 
            ICollection<BrandDto>? brands = await _productService.GetBrandCollectionAsync(input);
 
            response.Success = brands != null;
 
            if (!response.Success)
            {
                response.ErrorCode = "BRAND_COLLECTION.FIND.ERROR";
                return BadRequest(response);
            }
 
            response.Data = brands;
            return Ok(response);
        }
 
        // [HttpGet(template: "product/{productId}/color")]
        // [Authorize(Roles = "Shop")]
        // public async Task<ActionResult<ResponseModel<ICollection<ColorDto>>>> GetColorCollectionAsync([FromRoute] Guid productId)
        // {
        //     var response = new ResponseModel<ICollection<ColorDto>>();
 
        //     var colors = await _productService.GetColorCollectionAsync(productId);
 
        //     response.Success = colors != null;
 
        //     if (!response.Success)
        //     {
        //         response.ErrorCode = "COLOR_COLLECTION.FIND.ERROR";
        //         return BadRequest(response);
        //     }
 
        //     response.Data = colors;
        //     return Ok(response);
        // }
 
        [HttpGet(template: "product/{productId}")]
        public async Task<ActionResult<ResponseModel<ProductDto>>> GetProductAsync([FromRoute] Guid productId)
        {
            ResponseModel<ProductDto> response = new();
 
            ProductDto? product = await _productService.GetProductAsync(productId);
 
            response.Success = product != null;
 
            if (!response.Success)
            {
                response.ErrorCode = "PRODUCT.FIND.ERROR";
                return BadRequest(response);
            }
 
            response.Data = product;
            return Ok(response);
        }
 
        [HttpGet(template: "product")]
        public async Task<ActionResult<ResponseModel<ICollection<ProductDto>>>> GetProductCollectionAsync([FromQuery] FilterProductDto? input = null)
        {
            ResponseModel<ICollection<ProductDto>> response = new();
 
            ICollection<ProductDto>? products = await _productService.GetProductCollectionAsync(input);
 
            response.Success = products != null;
 
            if (!response.Success)
            {
                response.ErrorCode = "PRODUCT_COLLECTION.FIND.ERROR";
                return BadRequest(response);
            }
 
            response.Data = products;
            return Ok(response);
        }
 
        // [HttpGet(template: "product/{productId}/size")]
        // [Authorize(Roles = "Shop")]
        // public async Task<ActionResult<ResponseModel<ICollection<SizeDto>>>> GetSizeCollectionAsync([FromRoute] Guid productId)
        // {
        //     var response = new ResponseModel<ICollection<SizeDto>>();
 
        //     var sizes = await _productService.GetSizeCollectionAsync(productId);
 
        //     response.Success = sizes != null;
 
        //     if (!response.Success)
        //     {
        //         response.ErrorCode = "SIZE_COLLECTION.FIND.ERROR";
        //         return BadRequest(response);
        //     }
 
        //     response.Data = sizes;
        //     return Ok(response);
        // }
 
        [HttpDelete(template: "brand/{brandId}")]
        [Authorize(Roles = "Shop")]
        public async Task<ActionResult<ResponseModel<bool>>> RemoveBrandAsync([FromRoute] Guid brandId)
        {
            ResponseModel<bool> response = new();
 
            bool success = await _productService.RemoveBrandAsync(brandId);
 
            response.Success = success;
 
            if (!response.Success)
            {
                response.ErrorCode = "BRAND.DELETE.ERROR";
                return BadRequest(response);
            }
 
            return Ok(response);
        }
 
        [HttpDelete(template: "product/{productId}")]
        [Authorize(Roles = "Shop")]
        public async Task<ActionResult<ResponseModel<bool>>> RemoveProductAsync([FromRoute] Guid productId)
        {
            ResponseModel<bool> response = new();
 
            bool success = await _productService.RemoveProductAsync(productId);
 
            response.Success = success;
 
            if (!response.Success)
            {
                response.ErrorCode = "PRODUCT.DELETE.ERROR";
                return BadRequest(response);
            }
 
            return Ok(response);
        }
 
        [HttpDelete(template: "product/{productId}/color/{colorId}")]
        [Authorize(Roles = "Shop")]
        public async Task<ActionResult<ResponseModel<bool>>> RemoveProductColorAsync([FromRoute] Guid productId, [FromRoute] Guid colorId)
        {
            ResponseModel<bool> response = new();
 
            bool success = await _productService.RemoveProductColorAsync(productId, colorId);
 
            response.Success = success;
 
            if (!response.Success)
            {
                response.ErrorCode = "PRODUCT_COLOR.DELETE.ERROR";
                return BadRequest(response);
            }
 
            return Ok(response);
        }
 
        [HttpDelete(template: "product/{productId}/image/{imageId}")]
        [Authorize(Roles = "Shop")]
        public async Task<ActionResult<ResponseModel<bool>>> RemoveProductImageAsync([FromRoute] Guid productId, [FromRoute] Guid imageId)
        {
            ResponseModel<bool> response = new();
 
            bool success = await _productService.RemoveProductImageAsync(productId, imageId);
 
            response.Success = success;
 
            if (!response.Success)
            {
                response.ErrorCode = "PRODUCT_IMAGE.DELETE.ERROR";
                return BadRequest(response);
            }
 
            return Ok(response);
        }
 
        [HttpDelete(template: "product/{productId}/size/{sizeId}")]
        [Authorize(Roles = "Shop")]
        public async Task<ActionResult<ResponseModel<bool>>> RemoveProductSizeAsync([FromRoute] Guid productId, [FromRoute] Guid sizeId)
        {
            ResponseModel<bool> response = new();
 
            bool success = await _productService.RemoveProductSizeAsync(productId, sizeId);
 
            response.Success = success;
 
            if (!response.Success)
            {
                response.ErrorCode = "PRODUCT_SIZE.DELETE.ERROR";
                return BadRequest(response);
            }
 
            return Ok(response);
        }
 
        [HttpPut(template: "brand/{brandId}")]
        [Authorize(Roles = "Shop")]
        public async Task<ActionResult<ResponseModel<bool>>> UpdateBrandAsync([FromRoute] Guid brandId, [FromBody] UpdateBrandDto input)
        {
            ResponseModel<bool> response = new();
 
            bool success = await _productService.UpdateBrandAsync(brandId, input);
 
            response.Success = success;
 
            if (!response.Success)
            {
                response.ErrorCode = "BRAND.UPDATE.ERROR";
                return BadRequest(response);
            }
 
            return Ok(response);
        }
 
        [HttpPut(template: "brand/{brandId}/logo")]
        [Authorize(Roles = "Shop")]
        public async Task<ActionResult<ResponseModel<bool>>> UpdateBrandLogoAsync([FromRoute] Guid brandId, [FromForm] UpdateBrandLogoDto input)
        {
            ResponseModel<bool> response = new();
 
            bool success = await _productService.UpdateBrandLogoAsync(brandId, input);
 
            response.Success = success;
 
            if (!response.Success)
            {
                response.ErrorCode = "BRAND_LOGO.UPDATE.ERROR";
                return BadRequest(response);
            }
 
            return Ok(response);
        }
 
        [HttpPut(template: "product/{productId}")]
        [Authorize(Roles = "Shop")]
        public async Task<ActionResult<ResponseModel<bool>>> UpdateProductAsync([FromRoute] Guid productId, [FromBody] UpdateProductDto input)
        {
            ResponseModel<bool> response = new();
 
            bool success = await _productService.UpdateProductAsync(productId, input);
 
            response.Success = success;
 
            if (!response.Success)
            {
                response.ErrorCode = "PRODUCT.UPDATE.ERROR";
                return BadRequest(response);
            }
 
            return Ok(response);
        }
 
        [HttpPut(template: "product/{productId}/quantity")]
        [Authorize(Roles = "Shop")]
        public async Task<ActionResult<ResponseModel<bool>>> UpdateProductQuantityAsync([FromRoute] Guid productId, [FromBody] UpdateProductQuantityDto input)
        {
            ResponseModel<bool> response = new();
 
            bool success = await _productService.UpdateProductQuantityAsync(productId, input);
 
            response.Success = success;
 
            if (!response.Success)
            {
                response.ErrorCode = "PRODUCT_QUANTITY.UPDATE.ERROR";
                return BadRequest(response);
            }
 
            return Ok(response);
        }
    }
}
