using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Alterview.Web.Controllers
{
    [Route("api/sports")]
    public class SportsController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Boxing", "Unboxing", "Racing", "Unracing" }; // TODO STUB
        }
    }
}
