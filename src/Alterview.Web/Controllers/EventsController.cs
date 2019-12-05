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
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IEventsRepository _eventsRepo;

        public EventsController(ILogger<EventsController> logger, [FromServices]IEventsRepository eventsRepo)
        {
            _logger = logger;
            _eventsRepo = eventsRepo;
        }

        [HttpGet]
        [Route("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SportEvent>> Get(int id)
        {
            var ev = await _eventsRepo.GetEventById(id);

            if (ev != null)
            {
                return ev;
            }
            else
            {
                return NotFound();
            }
        }
    }
}
