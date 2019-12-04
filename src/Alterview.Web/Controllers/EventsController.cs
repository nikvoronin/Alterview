using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alterview.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Alterview.Web.Controllers
{
    [Route("api/events")]
    public class EventsController : Controller
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IEventsRepository _eventsRepo;

        public EventsController(ILogger<EventsController> logger, [FromServices]IEventsRepository eventsRepo)
        {
            _logger = logger;
            _eventsRepo = eventsRepo;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            IActionResult result = BadRequest();

            var ev = await _eventsRepo.GetEventById(id);

            if (ev != null)
                result = new JsonResult(ev);

            return result;
        }
    }

}
