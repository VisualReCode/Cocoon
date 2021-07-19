using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Web;
using MessagePack;
using ReCode.Cocoon.Legacy.Auth;

namespace ReCode.Cocoon.Legacy.Auth
{
    public class AuthApiHandler : IHttpHandler
    {
        [ExcludeFromCodeCoverage]
        public void ProcessRequest(HttpContext context)
        {
            ProcessRequest(new HttpContextWrapper(context));
        }
        
        public void ProcessRequest(HttpContextBase context)
        {
            if (context.User is ClaimsPrincipal principal)
            {
                if (principal.Identity?.IsAuthenticated == false)
                {
                    context.Response.Write("Not Authenticated");
                    context.Response.StatusCode = 401;
                    context.Response.Flush();
                    context.Response.End();
                    return;
                }

                var messagePrincipal = MessagePrincipal.FromClaimsPrincipal(principal);
                var bytes = MessagePackSerializer.Serialize(messagePrincipal);
                context.Response.BinaryWrite(bytes);
                context.Response.StatusCode = 200;
                context.Response.Flush();
                context.Response.End();
                return;
            }

            context.Response.StatusCode = 401;
            context.Response.Flush();
            context.Response.End();
        }

        public bool IsReusable => true;
    }
}