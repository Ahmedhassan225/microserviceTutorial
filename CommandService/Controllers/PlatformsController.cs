using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandService.Controllers
{
    [ApiController]
    [Route("api/c/[controller]")]
    public class PlatformController : ControllerBase
    {
        public PlatformController()
        {

        }

        [HttpPost()]
        public ActionResult TestInBoundConnection()
        {
            Console.WriteLine("--> InBound Post in Command Service");

            return Ok("Inbound test of from Platform Controller");
        }
    }
}
