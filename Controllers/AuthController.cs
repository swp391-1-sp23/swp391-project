using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SWP391.Project.Entities;
using SWP391.Project.Models;
using SWP391.Project.Models.Dtos.Login;
using SWP391.Project.Models.Dtos.Register;
using SWP391.Project.Services;

namespace SWP391.Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// User login with credential
        /// </summary>
        /// <param name="input">Email and Password</param>
        /// <returns></returns>
        [HttpPost(template: "login")]
        public async Task<ActionResult<ResponseModel<LoginOutput>>> LoginAsync([FromBody] LoginInput input)
        {
            var response = new ResponseModel<LoginOutput>();

            var result = await _authService.LoginAsync(input);

            if (result == null)
            {
                response.ErrorCode = "ACCOUNT.CREDENTIAL.INVALID";
                return response;
            };

            response.Success = result != null;
            response.Data = result;

            return response;
        }

        /// <summary>
        /// User register new account with credential
        /// </summary>
        /// <param name="input">Email, Password</param>
        /// <param name="role">Role</param>
        /// <returns></returns>
        [HttpPost(template: "register/{role}")]
        public async Task<ActionResult<ResponseModel<bool>>> RegisterAsync([FromBody] RegisterInput input, AccountRole role = AccountRole.Customer)
        {
            var response = new ResponseModel<bool>();

            var success = await _authService.RegisterAsync(input, role);

            if (!success)
            {
                response.ErrorCode = "ACCOUNT.CREATION.ERROR";
                return response;
            }

            response.Success = success;

            return response;
        }

        [HttpGet(template: "check")]
        [Authorize]
        public ActionResult<bool> Check()
        {
            return true;
        }
    }
}