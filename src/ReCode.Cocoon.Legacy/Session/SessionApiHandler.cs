using System;
using System.IO;
using System.Web;
using System.Web.SessionState;

namespace ReCode.Cocoon.Legacy.Session
{
    public class SessionApiHandler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            if (!string.Equals(context.Request.Path, "/facadesession"))
            {
                return;
            }

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
                var typeName = context.Request.QueryString["type"];
                if (string.IsNullOrEmpty(key))
                {
                    // Bad request
                    context.Response.StatusCode = 400;
                    return;
                }
                SetValue(context, key, typeName);
            }
        }

        private static void SetValue(HttpContext context, string key, string typeName)
        {
            var type = Type.GetType(typeName);

            if (type is null)
            {
                return;
            }

            var stream = context.Request.GetBufferlessInputStream();
            var bytes = new byte[128];
            var read = stream.Read(bytes, 0, 128);
            Array.Resize(ref bytes, read);

            var value = SessionValueDeserializer.Deserialize(type, bytes);
            context.Session[key] = value;
        }

        private static void GetValue(HttpContext context, string key)
        {
            var value = context.Session[key];
            if (value is null)
            {
                // Not found
                context.Response.StatusCode = 404;
                return;
            }

            var bytes = ValueSerializer.Serialize(value);

            context.Response.BinaryWrite(bytes);
            context.Response.StatusCode = 200;
        }

        public bool IsReusable => true;
    }
}