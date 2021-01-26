using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Text;
using MessagePack;
using MessagePack.Resolvers;

namespace ReCode.Cocoon.Proxy.Session
{
    internal static class SessionValueDeserializer
    {
        public static object Deserialize<T>(byte[] bytes)
        {
            if (Deserializers.TryGetValue(typeof(T), out var deserializer))
            {
                return deserializer(bytes);
            }

            return MessagePackSerializer.Deserialize<T>(bytes, TypelessContractlessStandardResolver.Options);
        }

        private static readonly Dictionary<Type, Func<byte[], object>> Deserializers = new()
        {
            [typeof(string)] = bytes => Encoding.UTF8.GetString(bytes),
            [typeof(short)] = bytes => BitConverter.ToInt16(bytes),
            [typeof(int)] = bytes => BitConverter.ToInt32(bytes),
            [typeof(long)] = bytes => BitConverter.ToInt64(bytes),
            [typeof(ushort)] = bytes => BitConverter.ToUInt16(bytes),
            [typeof(uint)] = bytes => BitConverter.ToUInt32(bytes),
            [typeof(ulong)] = bytes => BitConverter.ToUInt64(bytes),
            [typeof(bool)] = bytes => BitConverter.ToBoolean(bytes),
            [typeof(char)] = bytes => BitConverter.ToChar(bytes),
            [typeof(float)] = bytes => BitConverter.ToSingle(bytes),
            [typeof(double)] = bytes => BitConverter.ToDouble(bytes),
            [typeof(byte)] = bytes => bytes[0],
            [typeof(sbyte)] = bytes => ToSByte(bytes[0]),
            [typeof(decimal)] = bytes => GetDecimal(bytes),
            [typeof(DateTimeOffset)] = bytes => GetDateTimeOffset(bytes),
            [typeof(DateTime)] = bytes => GetDateTime(bytes),
            [typeof(TimeSpan)] = bytes => GetTimeSpan(bytes),
        };

        private static sbyte ToSByte(byte b)
        {
            unchecked
            {
                return (sbyte) b;
            }
        }
        
        public static DateTimeOffset GetDateTimeOffset(Span<byte> bytes)
        {
            return Utf8Parser.TryParse(bytes, out DateTimeOffset value, out _, 'O')
                ? value
                : throw new InvalidOperationException("Value was not a DateTimeOffset");
        }
        
        public static DateTime GetDateTime(Span<byte> bytes)
        {
            return Utf8Parser.TryParse(bytes, out DateTime value, out _, 'O')
                ? value
                : throw new InvalidOperationException("Value was not a DateTime");
        }
        
        public static TimeSpan GetTimeSpan(Span<byte> bytes)
        {
            return Utf8Parser.TryParse(bytes, out TimeSpan value, out _, 'c')
                ? value
                : throw new InvalidOperationException("Value was not a TimeSpan");
        }
        
        public static decimal GetDecimal(Span<byte> bytes)
        {
            return Utf8Parser.TryParse(bytes, out decimal value, out _, 'G')
                ? value
                : throw new InvalidOperationException("Value was not a decimal");
        }
    }
}