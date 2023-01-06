using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost(template: "login")]
        public async Task<ActionResult<ResponseModel<LoginOutput>>> Login([FromBody] LoginInput input)
        {
            var response = new ResponseModel<LoginOutput>();

            var result = await _authService.Login(email: input.Email, password: input.Password);

            if (result == null)
            {
                response.ErrorCode = "ACCOUNT.CREDENTIAL.INVALID";
                return response;
            };

            response.Success = result != null;
            response.Data = result;

            return response;
        }


        [HttpPost(template: "register")]
        public async Task<ActionResult<ResponseModel<bool>>> Register([FromBody] RegisterInput input)
        {
            var response = new ResponseModel<bool>();

            var success = await _authService.Register(email: input.Email, password: input.Password, role: input.Role);

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