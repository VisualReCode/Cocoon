using System;
using System.Buffers;
using System.Buffers.Text;

namespace ReCode.Cocoon.Legacy.Session
{
    public static class ValueSerializer
    {
        public static byte[] ToBytes(DateTimeOffset value)
        {
            Span<byte> bytes = stackalloc byte[40];
            Utf8Formatter.TryFormat(value, bytes, out int written, new StandardFormat('O'));
            var returnValue = new byte[written];
            bytes.Slice(0, written).CopyTo(returnValue);
            return returnValue;
        }
        
        public static byte[] ToBytes(DateTime value)
        {
            Span<byte> bytes = stackalloc byte[40];
            Utf8Formatter.TryFormat(value, bytes, out int written, new StandardFormat('O'));
            var returnValue = new byte[written];
            bytes.Slice(0, written).CopyTo(returnValue);
            return returnValue;
        }

        public static byte[] ToBytes(TimeSpan value)
        {
            Span<byte> bytes = stackalloc byte[40];
            Utf8Formatter.TryFormat(value, bytes, out int written, new StandardFormat('c'));
            var returnValue = new byte[written];
            bytes.Slice(0, written).CopyTo(returnValue);
            return returnValue;
        }

        public static byte[] ToBytes(decimal value)
        {
            Span<byte> bytes = stackalloc byte[40];
            Utf8Formatter.TryFormat(value, bytes, out int written, new StandardFormat('G'));
            var returnValue = new byte[written];
            bytes.Slice(0, written).CopyTo(returnValue);
            return returnValue;
        }
    }
}