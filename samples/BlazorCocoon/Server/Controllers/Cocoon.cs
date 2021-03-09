using Microsoft.AspNetCore.Mvc;

namespace BlazorCocoon.Server.Controllers
{
    public class CocoonController : Controller
    {
        private static readonly CocoonPrincipal Unauthenticated = new CocoonPrincipal
        {
            Name = string.Empty,
            IsAuthenticated = false
        };
        
        // GET
        [HttpGet("_cocoon/auth")]
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
    }
}