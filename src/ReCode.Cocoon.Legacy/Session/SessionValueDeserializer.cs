using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;
using MessagePack.Resolvers;

namespace ReCode.Cocoon.Legacy.Session
{
    internal static class SessionValueDeserializer
    {
        public static object Deserialize<T>(byte[] bytes)
        {
            if (Deserializers.TryGetValue(typeof(T), out var deserializer))
            {
                return deserializer(bytes);
            }

            return MessagePackSerializer.Deserialize<T>(bytes, ContractlessStandardResolver.Instance);
        }

        private static readonly Dictionary<Type, Func<byte[], object>> Deserializers = new Dictionary<Type, Func<byte[], object>>
        {
            [typeof(string)] = bytes => Encoding.UTF8.GetString(bytes),
            [typeof(short)] = bytes => BitConverter.ToInt16(bytes, 0),
            [typeof(int)] = bytes => BitConverter.ToInt32(bytes, 0),
            [typeof(long)] = bytes => BitConverter.ToInt64(bytes, 0),
            [typeof(ushort)] = bytes => BitConverter.ToUInt16(bytes, 0),
            [typeof(uint)] = bytes => BitConverter.ToUInt32(bytes, 0),
            [typeof(ulong)] = bytes => BitConverter.ToUInt64(bytes, 0),
            [typeof(bool)] = bytes => BitConverter.ToBoolean(bytes, 0),
            [typeof(char)] = bytes => BitConverter.ToChar(bytes, 0),
            [typeof(float)] = bytes => BitConverter.ToSingle(bytes, 0),
            [typeof(double)] = bytes => BitConverter.ToDouble(bytes, 0),
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
        
        public static DateTimeOffset GetDateTimeOffset(byte[] bytes)
        {
            var str = Encoding.UTF8.GetString(bytes);
            return DateTimeOffset.Parse(str);
        }
        
        public static DateTime GetDateTime(byte[] bytes)
        {
            var str = Encoding.UTF8.GetString(bytes);
            return DateTime.Parse(str);
        }
        
        public static TimeSpan GetTimeSpan(byte[] bytes)
        {
            var str = Encoding.UTF8.GetString(bytes);
            return TimeSpan.Parse(str);
        }
        
        public static decimal GetDecimal(byte[] bytes)
        {
            var str = Encoding.UTF8.GetString(bytes);
            return decimal.Parse(str);
        }
    }
}