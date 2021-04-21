using System;
using System.Text;
using ReCode.Cocoon.Legacy.Session;
using Xunit;

namespace ReCode.Cocoon.Legacy.Tests.Session
{
    public class ValueSerializerTests
    {
        [Fact]
        public void DateTimeOffsetOk()
        {
            var now = DateTimeOffset.UtcNow;

            var bytes = ValueSerializer.ToBytes(now);

            var str = Encoding.UTF8.GetString(bytes);
            Assert.Equal(now.ToString("O"), str);
        }
        
        [Fact]
        public void DateTimeOk()
        {
            var now = DateTime.UtcNow;

            var bytes = ValueSerializer.ToBytes(now);
            
            var str = Encoding.UTF8.GetString(bytes);
            Assert.Equal(now.ToString("O"), str);
        }

        [Fact]
        public void TimeSpanOk()
        {
            var now = DateTimeOffset.UtcNow.TimeOfDay;
            
            var bytes = ValueSerializer.ToBytes(now);
            
            var str = Encoding.UTF8.GetString(bytes);
            Assert.Equal(now.ToString("c"), str);
        }

        [Fact]
        public void DecimalOk()
        {
            var value = Convert.ToDecimal(Math.PI);
            
            var bytes = ValueSerializer.ToBytes(value);
            
            var str = Encoding.UTF8.GetString(bytes);
            Assert.Equal(value.ToString("G"), str);
        }
    }
}
