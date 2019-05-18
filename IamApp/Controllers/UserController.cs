using Microsoft.AspNetCore.Mvc;

namespace IamApp.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost("register")]
        public ActionResult Register()
        {
            return CreatedAtAction("TestMethod", "Hello from /user/register");
        }
    }
}