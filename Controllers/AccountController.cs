using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 
using SWP391.Project.Models;
using SWP391.Project.Models.Dtos.Account;
using SWP391.Project.Models.Dtos.Address;
using SWP391.Project.Models.Dtos.Cart;
using SWP391.Project.Models.Dtos.Image;
using SWP391.Project.Models.Dtos.Order;
using SWP391.Project.Models.Dtos.Product;
using SWP391.Project.Services;
 
namespace SWP391.Project.Controllers
{
    public interface IAccountController
    {
        // [Authorize]
        Task<ActionResult<ResponseModel<AccountDto>>> GetAccountAsync();
        // [Authorize(Roles = "Shop")]
        Task<ActionResult<ResponseModel<ICollection<AccountDto>>>> GetAccountCollectionAsync([FromQuery] FilterAccountDto filter);
        // [Authorize] OWN
        Task<ActionResult<ResponseModel<bool>>> UpdateAccountAsync([FromBody] UpdateAccountDto input);
        // [Authorize] OWN
        Task<ActionResult<ResponseModel<bool>>> UpdateAccountAvatarAsync([FromForm] UpdateAccountAvatarDto input);
        // [Authorize = "Shop"]
        Task<ActionResult<ResponseModel<bool>>> RemoveAccountAsync([FromRoute] Guid accountId);
        // [Authorize] OWN
        // Task<ActionResult<ResponseModel<AddressDto>>> GetAddressAsync([FromRoute] Guid addressId);
        // [Authorize] OWN
        Task<ActionResult<ResponseModel<ICollection<AddressDto>>>> GetAddressCollectionAsync();
        // [Authorize] OWN
        Task<ActionResult<ResponseModel<bool>>> AddAddressAsync([FromBody] AddAddressDto input);
        // [Authorize] OWN
        Task<ActionResult<ResponseModel<bool>>> RemoveAddressAsync([FromRoute] Guid addressId);
        // [Authorize] OWN
        Task<ActionResult<ResponseModel<ICollection<CartDto>>>> GetCartProductsAsync();
        // [Authorize] OWN
        Task<ActionResult<ResponseModel<bool>>> AddCartProductAsync([FromBody] AddCartProductDto input);
        // [Authorize] OWN
        Task<ActionResult<ResponseModel<bool>>> UpdateCartProductQuantityAsync([FromRoute] Guid cartId, [FromBody] UpdateCartProductQuantityDto input);
        // [Authorize] OWN
        // Task<ActionResult<ResponseModel<bool>>> RemoveCartProductAsync([FromRoute] Guid cartId);
        // [Authorize] OWN
        Task<ActionResult<ResponseModel<IDictionary<Guid, List<OrderDto>>>>> GetOrderCollectionAsync();
        // [Authorize] OWN
        Task<ActionResult<ResponseModel<ICollection<OrderDto>>>> GetOrderAsync([FromRoute] Guid orderTag);
        // [Authorize] OWN
        Task<ActionResult<ResponseModel<bool>>> UpdateOrderStatusAsync([FromRoute] Guid orderTag, [FromBody] UpdateOrderStatusDto input);
        // [Authorize] OWN
        // Task<ActionResult<ResponseModel<bool>>> AddFeedbackAsync([FromBody] AddFeedbackDto input);
        Task<ActionResult<ResponseModel<bool>>> AddOrderAsync(AddOrderDto input);
    }
 
    [ApiController]
    [Route("api")]
    public class AccountController : BaseController, IAccountController
    {
        private readonly IAccountService _accountService;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly IFeedbackService _feedbackService;
        private readonly IShipmentService _shipmentService;
 
        public AccountController(IAccountService accountService,
                                 ICartService cartService,
                                 IOrderService orderService,
                                 IFeedbackService feedbackService,
                                 IShipmentService shipmentService)
        {
            _accountService = accountService;
            _cartService = cartService;
            _orderService = orderService;
            _feedbackService = feedbackService;
            _shipmentService = shipmentService;
        }
 
        [HttpPost(template: "address")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<ResponseModel<bool>>> AddAddressAsync([FromBody] AddAddressDto input)
        {
            ResponseModel<bool> response = new();
 
            bool success = await _shipmentService.AddShipmentAddressAsync(AccountId, input);
 
            response.Success = success;
 
            if (!response.Success)
            {
                response.ErrorCode = "ADDRESS.CREATE.ERROR";
                return BadRequest(response);
            }
 
            return Ok(response);
        }
 
        [HttpPost(template: "cart")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<ResponseModel<bool>>> AddCartProductAsync([FromBody] AddCartProductDto input)
        {
            ResponseModel<bool> response = new();
 
            bool success = await _cartService.AddCartProductAsync(AccountId, input);
 
            response.Success = success;
 
            if (!response.Success)
            {
                response.ErrorCode = "CART.CREATE.ERROR";
                return BadRequest(response);
            }
 
            return Ok(response);
        }
 
        [HttpPost(template: "order")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<ResponseModel<bool>>> AddOrderAsync(AddOrderDto input)
        {
            ResponseModel<bool> response = new();
 
            bool success = await _orderService.AddOrderAsync(accountId: AccountId, input);
 
            response.Success = success;
 
            if (!response.Success)
            {
                response.ErrorCode = "ORDER.CREATE.ERROR";
                return BadRequest(response);
            }
 
            return Ok(response);
        }
 
        // [HttpPost(template: "account/feedback")]
        // [Authorize(Roles = "Customer")]
        // public async Task<ActionResult<ResponseModel<bool>>> AddFeedbackAsync([FromBody] AddFeedbackDto input)
        // {
        //     var response = new ResponseModel<bool>();
 
        //     var success = await _feedbackService.AddFeedbackAsync(AccountId, input: input);
 
        //     if (!success)
        //     {
        //         response.Success = false;
        //         response.ErrorCode = "FEEDBACK.CREATION.ERROR";
        //         return BadRequest(response);
        //     }
 
        //     response.Success = success;
 
        //     return Ok(response);
        // }
 
        [HttpGet(template: "profile")]
        public async Task<ActionResult<ResponseModel<AccountDto>>> GetAccountAsync()
        {
            ResponseModel<AccountDto> response = new();
 
            AccountDto? account = await _accountService.GetAccountAsync(AccountId);
 
            response.Success = account != null;
 
            if (!response.Success)
            {
                response.ErrorCode = "ACCOUNT.FIND.ERROR";
                return NotFound(response);
            }
 
            response.Data = account;
            return Ok(response);
        }
 
        [HttpGet(template: "account")]
        [Authorize(Roles = "Shop")]
        public async Task<ActionResult<ResponseModel<ICollection<AccountDto>>>> GetAccountCollectionAsync([FromQuery] FilterAccountDto? filter)
        {
            ResponseModel<ICollection<AccountDto>> response = new();
 
            ICollection<AccountDto>? accounts = await _accountService.GetAccountCollectionAsync(filter);
 
            response.Success = accounts != null;
 
            if (!response.Success)
            {
                response.ErrorCode = "ACCOUNT_COLLECTION.FIND.ERROR";
                return NotFound(response);
            }
 
            response.Data = accounts;
            return Ok(response);
        }
 
        // [HttpGet(template: "account/address/{addressId}")]
        // [Authorize]
        // public async Task<ActionResult<ResponseModel<AddressDto>>> GetAddressAsync([FromRoute] Guid addressId)
        // {
        //     var response = new ResponseModel<AddressDto>();
 
        //     var address = await _shipmentService.GetShipmentAddressAsync(addressId);
 
        //     if (address == null)
        //     {
        //         response.Success = false;
        //         response.ErrorCode = "ADDRESS.FIND.ERROR";
        //         return BadRequest(response);
        //     }
 
        //     response.Success = address != null;
        //     response.Data = address;
 
        //     return Ok(response);
        // }
 
        [HttpGet(template: "address")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<ResponseModel<ICollection<AddressDto>>>> GetAddressCollectionAsync()
        {
            ResponseModel<ICollection<AddressDto>> response = new();
 
            ICollection<AddressDto>? addresses = await _shipmentService.GetShipmentAddressCollectionAsync(AccountId);
 
            response.Success = addresses != null;
 
            if (!response.Success)
            {
                response.ErrorCode = "ADDRESS_COLLECTION.FIND.ERROR";
                return NotFound(response);
            }
 
            response.Data = addresses;
            return Ok(response);
        }
 
        [HttpGet(template: "cart")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<ResponseModel<ICollection<CartDto>>>> GetCartProductsAsync()
        {
            ResponseModel<ICollection<CartDto>> response = new();
 
            ICollection<CartDto>? products = await _cartService.GetProductCollectionByAccountIdAsync(AccountId);
 
            response.Success = products != null;
 
            if (!response.Success)
            {
                response.ErrorCode = "PRODUCT_COLLECTION.FIND.ERROR";
                return NotFound(response);
            }
 
            response.Data = products;
            return Ok(response);
        }
 
        [HttpGet(template: "order/{orderTag}")]
        [Authorize]
        public async Task<ActionResult<ResponseModel<ICollection<OrderDto>>>> GetOrderAsync([FromRoute] Guid orderTag)
        {
            ResponseModel<ICollection<OrderDto>> response = new();
 
            ICollection<OrderDto>? orders = await _orderService.GetAccountOrderAsync(AccountId, orderTag);
 
            response.Success = orders != null;
 
            if (!response.Success)
            {
                response.ErrorCode = "ORDER.FIND.ERROR";
                return NotFound(response);
            }
 
            response.Data = orders;
            return Ok(response);
        }
 
        [HttpGet(template: "order")]
        [Authorize]
        public async Task<ActionResult<ResponseModel<IDictionary<Guid, List<OrderDto>>>>> GetOrderCollectionAsync()
        {
            ResponseModel<IDictionary<Guid, List<OrderDto>>> response = new();
 
            IDictionary<Guid, List<OrderDto>>? orders = await _orderService.GetAccountOrderCollectionAsync(AccountId);
 
            response.Success = orders != null;
 
            if (!response.Success)
            {
                response.ErrorCode = "ORDER_COLLECTION.FIND.ERROR";
                return NotFound(response);
            }
 
            response.Data = orders;
            return Ok(response);
        }
 
        [HttpDelete(template: "account/{accountId}")]
        [Authorize(Roles = "Shop")]
        public async Task<ActionResult<ResponseModel<bool>>> RemoveAccountAsync([FromRoute] Guid accountId)
        {
            ResponseModel<bool> response = new();
 
            bool success = await _accountService.RemoveCustomerAccountAsync(accountId);
 
            response.Success = success;
 
            if (!success)
            {
                response.ErrorCode = "ACCOUNT.DELETE.ERROR";
                return BadRequest(response);
            }
 
            return Ok(response);
        }
 
        [HttpDelete(template: "address/{addressId}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<ResponseModel<bool>>> RemoveAddressAsync([FromRoute] Guid addressId)
        {
            ResponseModel<bool> response = new();
 
            bool success = await _shipmentService.RemoveShipmentAddressAsync(AccountId, addressId);
 
            response.Success = success;
 
            if (!response.Success)
            {
                response.ErrorCode = "ADDRESS.DELETE.ERROR";
                return BadRequest(response);
            }
 
            return Ok(response);
        }
 
        // [HttpDelete(template: "account/cart/{cartId}")]
        // public Task<ActionResult<ResponseModel<bool>>> RemoveCartProductAsync([FromRoute] Guid cartId)
        // {
        //     var response = new ResponseModel<bool>();
 
        //     var success = _cartService.(cartId);
 
        //     throw new NotImplementedException();
        // }
 
        [HttpPut(template: "profile")]
        [Authorize]
        public async Task<ActionResult<ResponseModel<bool>>> UpdateAccountAsync([FromBody] UpdateAccountDto input)
        {
            ResponseModel<bool> response = new();
 
            bool success = await _accountService.UpdateAccountAsync(AccountId, input);
 
            response.Success = success;
 
            if (!response.Success)
            {
                response.ErrorCode = "ACCOUNT.UPDATE.ERROR";
                return BadRequest(response);
            }
 
            return Ok(response);
        }
 
        [HttpPost(template: "avatar")]
        [Authorize]
        public async Task<ActionResult<ResponseModel<bool>>> UpdateAccountAvatarAsync([FromForm] UpdateAccountAvatarDto input)
        {
            ResponseModel<bool> response = new();
 
            bool success = await _accountService.UpdateAccountAvatarAsync(AccountId, input);
 
            response.Success = success;
 
            if (!response.Success)
            {
                response.ErrorCode = "AVATAR.UPDATE.ERROR";
                return BadRequest(response);
            }
 
            return Ok(response);
        }
 
        [HttpPut(template: "cart/{cartId}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<ResponseModel<bool>>> UpdateCartProductQuantityAsync([FromRoute] Guid cartId, [FromBody] UpdateCartProductQuantityDto input)
        {
            ResponseModel<bool> response = new();
 
            bool success = await _cartService.UpdateCartProductQuantityAsync(AccountId, input);
 
            response.Success = success;
 
            if (!response.Success)
            {
                response.ErrorCode = "CART_PRODUCT.UPDATE.ERROR";
                return BadRequest(response);
            }
 
            return Ok(response);
        }
 
        [HttpPut(template: "order/{orderTag}")]
        [Authorize]
        public async Task<ActionResult<ResponseModel<bool>>> UpdateOrderStatusAsync([FromRoute] Guid orderTag, [FromBody] UpdateOrderStatusDto input)
        {
            ResponseModel<bool> response = new();
 
            bool success = await _orderService.UpdateOrderStatusAsync(AccountId, orderTag, input);
 
            response.Success = success;
 
            if (!response.Success)
            {
                response.ErrorCode = "ORDER.UPDATE.ERROR";
                return BadRequest(response);
            }
 
            return Ok(response);
        }
    }
}
