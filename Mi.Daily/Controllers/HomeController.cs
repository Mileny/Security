using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mi.Daily.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase {
        public ActionResult Index() {
            return Ok("Daily");
        }
    }
}
