using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alterview.Core.Interfaces;
using Alterview.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Alterview.Web.Controllers
{
    [ApiController]
    [Route("api/sports")]
    public class SportsController : ControllerBase
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<SportEvent>>> Get(int sportId, DateTime dt)
        {
            var events = await _eventsRepo.GetEventsBySportAndDate(sportId, dt) as List<SportEvent>;

            if (events != null && events.Count > 0)
            {
                return events;
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<SportInfo>>> Get()
        {
            var sports = await _sportRepo.GetSportsWithEventsCount() as List<SportInfo>;

            if (sports != null && sports.Count > 0)
            {
                return sports;
            }
            else
            {
                return NotFound();
            }
        }
    }
}
