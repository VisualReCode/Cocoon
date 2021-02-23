using Microsoft.AspNetCore.Mvc;

namespace MvcCocoon.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        // GET
        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(".AspNet.ApplicationCookie");
            return Redirect("/");
        }
    }
}