using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcCocoon.Models;
using ReCode.Cocoon.Proxy.Cookies;
using ReCode.Cocoon.Proxy.Session;
using SharedStuff;

namespace MvcCocoon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CocoonSession _session;
        private readonly ICocoonCookies _cookies;

        public HomeController(ILogger<HomeController> logger, CocoonSession session, ICocoonCookies cookies)
        {
            _logger = logger;
            _session = session;
            _cookies = cookies;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            await _cookies.SetAsync("Foo", "This is my cookie value");
            _session.Set("Answer", 42);
            return View();
        }

        [HttpGet("privacy")]
        public async Task<ActionResult> Privacy()
        {
            var foo = await _session.GetAsync<Foo>("foo");
            foo.Bar = "quux";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("hack")]
        public async Task<string> GetHack()
        {
            var cartId = await _session.GetAsync<string>("CartId");
            return cartId;
        }
    }
}
