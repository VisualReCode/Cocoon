using System;
using ReCode.Cocoon.Legacy.Session;
using Xunit;

namespace ReCode.Cocoon.Legacy.Tests.Session
{
    public class SessionValueDeserializerTests
    {
        [Fact]
        public void DeserializesInt()
        {
            var bytes = BitConverter.GetBytes(42);
            var actual = SessionValueDeserializer.Deserialize(typeof(int), bytes);
            Assert.IsType<int>(actual);
            Assert.Equal(42, (int)actual);
        }

        [Fact]
        public void GetsInt32FromNet50Type()
        {
            const string net50int = "System.Int32, System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";
            Assert.True(SessionValueDeserializer.GetTypeFromName(net50int, out var type));
            Assert.Equal(typeof(int), type);
        }
    }
}