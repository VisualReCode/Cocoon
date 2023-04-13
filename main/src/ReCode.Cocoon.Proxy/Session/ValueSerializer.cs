using System;
using System.Reflection;
using System.Text;
using MessagePack;
using MessagePack.Resolvers;

namespace ReCode.Cocoon.Proxy.Session
{
    public static class ValueSerializer
    {
        private static readonly byte[] Empty = Array.Empty<byte>();
        public static byte[] Serialize(object? value)
        {
            switch (value)
            {
                case null:
                    return Empty;
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
                    return ToBytes(d);
                case DateTimeOffset dto:
                    return ToBytes(dto);
                case DateTime dt:
                    return ToBytes(dt);
                case TimeSpan ts:
                    return ToBytes(ts);
                default:
                    return MessagePack(value);
            }
        }
        
        private static byte[] MessagePack(object value)
        {
            if (value.GetType().GetCustomAttribute(typeof(MessagePackObjectAttribute)) != null)
            {
                return MessagePackSerializer.Serialize(value);
            }

            return MessagePackSerializer.Serialize(value, ContractlessStandardResolver.Options);
        }


        public static byte[] ToBytes(DateTimeOffset value)
        {
            return Encoding.UTF8.GetBytes(value.ToString("O"));
        }
        
        public static byte[] ToBytes(DateTime value)
        {
            return Encoding.UTF8.GetBytes(value.ToString("O"));
        }
        
        public static byte[] ToBytes(TimeSpan value)
        {
            return Encoding.UTF8.GetBytes(value.ToString("c"));
        }
        
        public static byte[] ToBytes(decimal value)
        {
            return Encoding.UTF8.GetBytes(value.ToString("G"));
        }
    }
}