using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alterview.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Alterview.Web.Controllers
{
    [Route("api/sports")]
    public class SportsController : Controller
    {
        private readonly ILogger<SportsController> _logger;
        private readonly IEventsRepository _eventsRepo;
        private readonly ISportsRepository _sportRepo;

        public SportsController(
            ILogger<SportsController> logger,
            [FromServices]IEventsRepository eventsRepo,
            [FromServices]ISportsRepository sportRepo)
        {
            _logger = logger;
            _eventsRepo = eventsRepo;
            _sportRepo = sportRepo;
        }

        [HttpGet]
        [Route("{sportId}/events/date/{dt}")]
        public async Task<IActionResult> Get(int sportId, DateTime dt)
        {
            IActionResult result = BadRequest();

            var events = await _eventsRepo.GetEventsBySportAndDate(sportId, dt);

            if (events != null)
                result = new JsonResult(events);

            return result;
        }

        [HttpGet]
        [Route("info")]
        public async Task<IActionResult> GetAll()
        {
            IActionResult result = BadRequest();

            var sports = await _sportRepo.GetSportsWithEventsCount();

            if (sports != null)
                result = new JsonResult(sports);

            return result;
        }
    }
}
