using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;

using SWP391.Project.Entities;

namespace SWP391.Project.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected Guid AccountId => Guid.Parse(input: User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        protected AccountRole AccountRole => (AccountRole)Enum.Parse(typeof(AccountRole), User.FindFirst(ClaimTypes.Role)!.Value);

        protected bool IsAccountOwner(Guid accountId)
        {
            return accountId == AccountId;
        }
    }
}