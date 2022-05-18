using Microsoft.AspNetCore.Mvc;

namespace DMX.Core.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get() =>
            Ok("Hi, Zuko here...");
    }
}
