using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace ReCode.Cocoon.Legacy.Cookies
{
    public class CookieApiHandler : IHttpHandler, IRequiresSessionState
    {
        [ExcludeFromCodeCoverage]
        public void ProcessRequest(HttpContext context)
        {
            ProcessRequest(new HttpContextWrapper(context));
        }
        
        public void ProcessRequest(HttpContextBase context)
        {
            var key = context.Request.QueryString["key"];

            if (string.IsNullOrEmpty(key))
            {
                // Bad request
                context.Response.StatusCode = 400;
                return;
            }

            if ("GET".Equals(context.Request.HttpMethod, StringComparison.OrdinalIgnoreCase))
            {
                GetValue(context, key);
            }
            else if ("PUT".Equals(context.Request.HttpMethod, StringComparison.OrdinalIgnoreCase))
            {
                SetValue(context, key);
            }
        }
        
        private static void SetValue(HttpContextBase context, string key)
        {
            var stream = context.Request.GetBufferedInputStream();
            var reader = new StreamReader(stream);
            var value = reader.ReadToEnd();
            context.Response.Cookies.Set(new HttpCookie(key, value));
            context.Response.StatusCode = 200;
            context.Response.End();
        }

        private static void GetValue(HttpContextBase context, string key)
        {
            var cookie = context.Request.Cookies.Get(key);
            if (cookie is null)
            {
                // Not found
                context.Response.StatusCode = 404;
                return;
            }

            var bytes = Encoding.UTF8.GetBytes(cookie.Value);

            context.Response.BinaryWrite(bytes);
            context.Response.StatusCode = 200;
        }

        public bool IsReusable => true;
    }
}