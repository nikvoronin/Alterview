using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Alterview.Web.Controllers
{
    [Route("api/events")]
    public class EventsController : Controller
    {
        [HttpGet("{id:min(1)}")]
        public string Get(int id)
        {
            return $"STUB {id}"; // TODO STUB
        }
    }
}
