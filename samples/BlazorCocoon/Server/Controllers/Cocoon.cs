using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReCode.Cocoon.Proxy.Session;

namespace BlazorCocoon.Server.Controllers
{
    [Route("_cocoon")]
    public class CocoonController : Controller
    {
        private static readonly CocoonPrincipal Unauthenticated = new CocoonPrincipal
        {
            Name = string.Empty,
            IsAuthenticated = false
        };

        private readonly CocoonSession _session;
        
        public CocoonController(CocoonSession session)
        {
            _session = session;
        }
        
        // GET
        [HttpGet("auth")]
        public ActionResult<CocoonPrincipal> Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return new CocoonPrincipal
                {
                    Name = User.Identity.Name,
                    IsAuthenticated = true
                };
            }

            return Unauthenticated;
        }
        
        [HttpGet("session")]
        public async Task<string> Get(string id)
        {
            return await _session.GetAsync<string>(id);
        }

        [HttpPost("session")]
        public void Save(string id, string value)
        {
            _session.Set(id, value);
        }
    }
}