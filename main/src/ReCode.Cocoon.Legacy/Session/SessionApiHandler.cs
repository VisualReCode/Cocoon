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
                if (string.IsNullOrEmpty(typeName))
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
            if (SessionValueDeserializer.GetTypeFromName(typeName, out var type)) return;

            var stream = context.Request.GetBufferlessInputStream();

            var length = context.Request.ContentLength;
            byte[] bytes;
            using (var reader = new BinaryReader(stream))
            {
                bytes = reader.ReadBytes(length);
            }

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