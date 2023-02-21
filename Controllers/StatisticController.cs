using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using swp391-project.Project.Services;

namespace swp391-project.Controllers
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