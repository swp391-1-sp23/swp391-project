using Microsoft.AspNetCore.Mvc;

using SWP391.Project.Models.Dtos.Account;
using SWP391.Project.Models.Dtos.Address;
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
        Task<ActionResult> GetAccountAsync();
        // [Authorize(Roles = "Shop")]
        Task<ActionResult> GetAccountCollectionAsync([FromQuery] FilterAccountDto filter);
        // [Authorize] OWN
        Task<ActionResult> UpdateAccountAsync([FromBody] UpdateAccountDto input);
        // [Authorize] OWN
        Task<ActionResult> UpdateAccountAvatarAsync([FromForm] UpdateAccountAvatarDto input);
        // [Authorize = "Shop"]
        Task<ActionResult> RemoveAccountAsync([FromRoute] Guid accountId);
        // [Authorize] OWN
        Task<ActionResult> GetAddressAsync([FromRoute] Guid addressId);
        // [Authorize] OWN
        Task<ActionResult> GetAddressCollectionAsync();
        // [Authorize] OWN
        Task<ActionResult> AddAddressAsync([FromBody] AddAddressDto input);
        // [Authorize] OWN
        Task<ActionResult> RemoveAddressAsync([FromRoute] Guid addressId);
        // [Authorize] OWN
        Task<ActionResult> GetCartProductsAsync();
        // [Authorize] OWN
        Task<ActionResult> AddCartProductAsync([FromBody] AddCartProductDto input);
        // [Authorize] OWN
        Task<ActionResult> UpdateCartProductQuantityAsync([FromBody] UpdateCartProductQuantityDto input);
        // [Authorize] OWN
        Task<ActionResult> RemoveCartProductAsync([FromRoute] Guid cartId);
        // [Authorize] OWN
        Task<ActionResult> GetOrderCollectionAsync([FromQuery] FilterOrderDto input);
        // [Authorize] OWN
        Task<ActionResult> GetOrderAsync([FromRoute] Guid orderTag);
        // [Authorize] OWN
        Task<ActionResult> UpdateOrderStatusAsync([FromBody] UpdateOrderStatusDto input);
        // [Authorize] OWN
        Task<ActionResult> AddFeedbackAsync([FromBody] AddFeedbackDto input);
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase, IAccountController
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

        public Task<ActionResult> AddAddressAsync([FromBody] AddAddressDto input)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> AddCartProductAsync([FromBody] AddCartProductDto input)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> AddFeedbackAsync([FromBody] AddFeedbackDto input)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> GetAccountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> GetAccountCollectionAsync([FromQuery] FilterAccountDto filter)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> GetAddressAsync([FromRoute] Guid addressId)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> GetAddressCollectionAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> GetCartProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> GetOrderAsync([FromRoute] Guid orderTag)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> GetOrderCollectionAsync([FromQuery] FilterOrderDto input)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> RemoveAccountAsync([FromRoute] Guid accountId)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> RemoveAddressAsync([FromRoute] Guid addressId)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> RemoveCartProductAsync([FromRoute] Guid cartId)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> UpdateAccountAsync([FromBody] UpdateAccountDto input)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> UpdateAccountAvatarAsync([FromForm] UpdateAccountAvatarDto input)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> UpdateCartProductQuantityAsync([FromBody] UpdateCartProductQuantityDto input)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> UpdateOrderStatusAsync([FromBody] UpdateOrderStatusDto input)
        {
            throw new NotImplementedException();
        }
    }
}