using System;
using System.Text;
using System.Web;
using System.Web.SessionState;
using MessagePack;
using MessagePack.Resolvers;

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

            var value = context.Session[key];
            if (value is null)
            {
                // Not found
                context.Response.StatusCode = 404;
                return;
            }

            var bytes = Serialize(value);
            
            context.Response.BinaryWrite(bytes);
            context.Response.StatusCode = 200;
        }

        private static byte[] Serialize(object value)
        {
            switch (value)
            {
                case string str:
                    return Encoding.UTF8.GetBytes(str);
                case int i32:
                    return BitConverter.GetBytes(i32);
                case long i64:
                    return BitConverter.GetBytes(i64);
                default:
                    return MessagePackSerializer.Serialize(value, ContractlessStandardResolver.Instance);
            }
        }

        public bool IsReusable => true;
    }
}