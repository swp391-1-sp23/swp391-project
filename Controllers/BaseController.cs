using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;

using SWP391.Project.Models;

namespace SWP391.Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected Guid AccountId => Guid.Parse(input: User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        protected bool IsAccountOwner(Guid accountId) => accountId == AccountId;
    }
}