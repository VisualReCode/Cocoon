using System;
using System.Globalization;
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
                case short i16:
                    return BitConverter.GetBytes(i16);
                case int i32:
                    return BitConverter.GetBytes(i32);
                case long i64:
                    return BitConverter.GetBytes(i64);
                case ushort u16:
                    return BitConverter.GetBytes(u16);
                case uint u32:
                    return BitConverter.GetBytes(u32);
                case ulong u64:
                    return BitConverter.GetBytes(u64);
                case char c:
                    return BitConverter.GetBytes(c);
                case bool b:
                    return BitConverter.GetBytes(b);
                case float f:
                    return BitConverter.GetBytes(f);
                case double d:
                    return BitConverter.GetBytes(d);
                case byte b:
                    return new[] {b};
                case sbyte sb:
                    return new[] {(byte)sb};
                case decimal d:
                    return ValueSerializer.ToBytes(d);
                case DateTimeOffset dto:
                    return ValueSerializer.ToBytes(dto);
                case DateTime dt:
                    return ValueSerializer.ToBytes(dt);
                case TimeSpan ts:
                    return ValueSerializer.ToBytes(ts);
                default:
                    return MessagePackSerializer.Serialize(value, ContractlessStandardResolver.Instance);
            }
        }
        
        private static byte[] DecimalToBytes(decimal value)
        {
            return Encoding.UTF8.GetBytes(value.ToString(CultureInfo.InvariantCulture));
        }
        
        private static sbyte ToSByte(byte b)
        {
            unchecked
            {
                return (sbyte) b;
            }
        }

        public bool IsReusable => true;
    }
}