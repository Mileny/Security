using Microsoft.AspNetCore.Mvc;

namespace Mi.UserCenter.Controllers {

    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase {

        public ActionResult Index() {
            return Ok("成功");
        }
    }
}
