using System;
using System.Text;

// using System.Buffers;
// using System.Buffers.Text;

namespace ReCode.Cocoon.Legacy.Session
{
    public static class ValueSerializer
    {
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