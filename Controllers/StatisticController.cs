using Microsoft.AspNetCore.Mvc;

using SWP391.Project.Services;

namespace SWP391.Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticService _statisticService;

        public StatisticController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }
    }
}