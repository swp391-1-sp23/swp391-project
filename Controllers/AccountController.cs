using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SWP391.Project.Models;
using SWP391.Project.Models.Dtos.Account;
using SWP391.Project.Models.Dtos.Address;
using SWP391.Project.Models.Dtos.Cart;
using SWP391.Project.Models.Dtos.Feedback;
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
        Task<ActionResult<ResponseModel<bool>>> UpdateCartProductQuantityAsync([FromBody] UpdateCartProductQuantityDto input);
        // [Authorize] OWN
        Task<ActionResult<ResponseModel<bool>>> RemoveCartProductAsync([FromRoute] Guid cartId);
        // [Authorize] OWN
        Task<ActionResult<ResponseModel<ICollection<IGrouping<Guid, OrderDto>>>>> GetOrderCollectionAsync();
        // [Authorize] OWN
        Task<ActionResult<ResponseModel<ICollection<OrderDto>>>> GetOrderAsync([FromRoute] Guid orderTag);
        // [Authorize] OWN
        Task<ActionResult<ResponseModel<bool>>> UpdateOrderStatusAsync([FromBody] UpdateOrderStatusDto input);
        // [Authorize] OWN
        Task<ActionResult<ResponseModel<bool>>> AddFeedbackAsync([FromBody] AddFeedbackDto input);
    }

    [ApiController]
    [Route("api/[controller]")]
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

        [HttpPost(template: "account/address")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<ResponseModel<bool>>> AddAddressAsync([FromBody] AddAddressDto input)
        {
            var response = new ResponseModel<bool>();

            if (!IsAccountOwner(input.AccountId))
            {
                response.Success = false;
                response.ErrorCode = "CREDENTIAL.VALIDATION.ERROR";
                return Unauthorized(response);
            }

            var success = await _shipmentService.AddShipmentAddressAsync(input: input);

            if (!success)
            {
                response.Success = false;
                response.ErrorCode = "ADDRESS.CREATION.ERROR";
                return BadRequest(response);
            }

            response.Success = success;

            return Ok(response);
        }

        [HttpPost(template: "account/cart")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<ResponseModel<bool>>> AddCartProductAsync([FromBody] AddCartProductDto input)
        {
            var response = new ResponseModel<bool>();

            if (!IsAccountOwner(input.AccountId))
            {
                response.Success = false;
                response.ErrorCode = "CREDENTIAL.VALIDATION.ERROR";
                return BadRequest(response);
            }

            var success = await _cartService.AddCartProductAsync(input: input);

            if (!success)
            {
                response.Success = false;
                response.ErrorCode = "CART.CREATION.ERROR";
                return BadRequest(response);
            }

            response.Success = success;

            return Ok(response);
        }

        [HttpPost(template: "account/feedback")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<ResponseModel<bool>>> AddFeedbackAsync([FromBody] AddFeedbackDto input)
        {
            var response = new ResponseModel<bool>();

            if (!IsAccountOwner(input.AccountId))
            {
                response.Success = false;
                response.ErrorCode = "CREDENTIAL.VALIDATION.ERROR";
                return BadRequest(response);
            }
var success = await _feedbackService.AddFeedbackAsync(input: input);

            if (!success)
            {
                response.Success = false;
                response.ErrorCode = "FEEDBACK.CREATION.ERROR";
                return BadRequest(response);
            }

            response.Success = success;

            return Ok(response);
        }

        [HttpGet(template: "account/profile")]
        [Authorize]
        public async Task<ActionResult<ResponseModel<AccountDto>>> GetAccountAsync()
        {
            var response = new ResponseModel<AccountDto>();

            var account = await _accountService.GetAccountAsync(AccountId);

            if (account == null)
            {
                response.Success = false;
                response.ErrorCode = "ACCOUNT.FIND.ERROR";
                return NotFound(response);
            }

            response.Success = account != null;
            response.Data = account;

            return Ok(response);
        }

        [HttpGet(template: "account")]
        [Authorize(Roles = "Shop")]
        public async Task<ActionResult<ResponseModel<ICollection<AccountDto>>>> GetAccountCollectionAsync([FromQuery] FilterAccountDto? filter)
        {
            var response = new ResponseModel<ICollection<AccountDto>>();

            var accounts = await _accountService.GetAccountCollectionAsync(filter ?? new());

            if (accounts == null)
            {
                response.Success = false;
                response.ErrorCode = "ACCOUNT_COLLECTION.FIND.ERROR";
                return NotFound(response);
            }

            response.Success = accounts != null;
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

        [HttpGet(template: "account/address")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<ResponseModel<ICollection<AddressDto>>>> GetAddressCollectionAsync()
        {
            var response = new ResponseModel<ICollection<AddressDto>>();

            var addresses = await _shipmentService.GetShipmentAddressCollectionAsync(AccountId);

            if (addresses == null)
            {
                response.Success = false;
response.ErrorCode = "ADDRESS_COLLECTION.FIND.ERROR";
                return BadRequest(response);
            }

            response.Success = addresses != null;
            response.Data = addresses;

            return Ok(response);
        }

        [HttpGet(template: "account/cart")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<ResponseModel<ICollection<CartDto>>>> GetCartProductsAsync()
        {
            var response = new ResponseModel<ICollection<CartDto>>();

            var products = await _cartService.GetProductCollectionByAccountIdAsync(AccountId);

            if (products == null)
            {
                response.Success = false;
                response.ErrorCode = "PRODUCT_COLLECTION.FIND.ERROR";
                return BadRequest(response);
            }

            response.Success = products != null;
            response.Data = products;

            return Ok(response);
        }

        [HttpGet(template: "account/order/{orderTag}")]
        [Authorize]
        public async Task<ActionResult<ResponseModel<ICollection<OrderDto>>>> GetOrderAsync([FromRoute] Guid orderTag)
        {
            var response = new ResponseModel<ICollection<OrderDto>>();

            var orders = await _orderService.GetAccountOrderAsync(AccountId, orderTag);

            if (orders == null)
            {
                response.Success = false;
                response.ErrorCode = "ORDER.FIND.ERROR";
                return NotFound(response);
            }

            response.Success = orders != null;
            response.Data = orders;

            return Ok(response);
        }

        [HttpGet(template: "account/order")]
        [Authorize]
        public async Task<ActionResult<ResponseModel<ICollection<IGrouping<Guid, OrderDto>>>>> GetOrderCollectionAsync()
        {
            var response = new ResponseModel<ICollection<IGrouping<Guid, OrderDto>>>();

            var orders = await _orderService.GetAccountOrderCollectionAsync(AccountId);

            if (orders == null)
            {
                response.Success = false;
                response.ErrorCode = "ORDER_COLLECTION.FIND.ERROR";
                return NotFound(response);
            }

            response.Success = orders != null;
            response.Data = orders;

            return Ok(response);
        }

        [HttpDelete(template: "account/{accountId}")]
        [Authorize(Roles = "Shop")]
        public async Task<ActionResult<ResponseModel<bool>>> RemoveAccountAsync([FromRoute] Guid accountId)
        {
            var response = new ResponseModel<bool>();

            var success = await _accountService.RemoveCustomerAccountAsync(accountId);

            if (!success)
            {
                response.Success = false;
                response.ErrorCode = "ACCOUNT.DELETION.ERROR";
                return BadRequest(response);
            }

            response.Success = success;
return Ok(response);
        }

        [HttpDelete(template: "account/address/{addressId}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<ResponseModel<bool>>> RemoveAddressAsync([FromRoute] Guid addressId)
        {
            var response = new ResponseModel<bool>();

            if (!IsAccountOwner(AccountId))
            {
                response.Success = false;
                response.ErrorCode = "CREDENTIAL.VALIDATION.ERROR";
                return BadRequest(response);
            }

            var success = await _shipmentService.RemoveShipmentAddressAsync(addressId);

            if (!success)
            {
                response.Success = false;
                response.ErrorCode = "ADDRESS.DELETION.ERROR";
                return BadRequest(response);
            }

            response.Success = success;

            return Ok(response);
        }

        [HttpDelete(template: "account/cart/{cartId}")]
        public Task<ActionResult<ResponseModel<bool>>> RemoveCartProductAsync([FromRoute] Guid cartId)
        {
            var response = new ResponseModel<bool>();
            throw new NotImplementedException();
        }

        [HttpPut(template: "account/profile")]
        public Task<ActionResult<ResponseModel<bool>>> UpdateAccountAsync([FromBody] UpdateAccountDto input)
        {
            throw new NotImplementedException();
        }

        [HttpPost(template: "account/avatar")]
        public Task<ActionResult<ResponseModel<bool>>> UpdateAccountAvatarAsync([FromForm] UpdateAccountAvatarDto input)
        {
            throw new NotImplementedException();
        }

        [HttpPut(template: "account/cart/{cartId}")]
        public Task<ActionResult<ResponseModel<bool>>> UpdateCartProductQuantityAsync([FromBody] UpdateCartProductQuantityDto input)
        {
            throw new NotImplementedException();
        }

        [HttpPut(template: "account/feedback/{feedbackId}")]
        public Task<ActionResult<ResponseModel<bool>>> UpdateOrderStatusAsync([FromBody] UpdateOrderStatusDto input)
        {
            throw new NotImplementedException();
        }
    }
}
