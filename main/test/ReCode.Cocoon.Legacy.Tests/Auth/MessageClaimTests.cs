using System.Linq;
using ReCode.Cocoon.Legacy.Auth;
using Xunit;

namespace ReCode.Cocoon.Legacy.Tests.Auth
{
    public class MessageClaimTests
    {
        [Fact]
        public void AttributesAndOrderCorrectlyApplied()
        {
            var props = typeof(MessageClaim).GetProperties();
            Assert.True(AttributeHelper.AttributeCorrect(props.Single(x => x.Name == "Issuer"),0));
            Assert.True(AttributeHelper.AttributeCorrect(props.Single(x => x.Name == "OriginalIssuer"),1));
            Assert.True(AttributeHelper.AttributeCorrect(props.Single(x => x.Name == "Properties"),2));
            Assert.True(AttributeHelper.AttributeCorrect(props.Single(x => x.Name == "Type"),3));
            Assert.True(AttributeHelper.AttributeCorrect(props.Single(x => x.Name == "Value"),4));
            Assert.True(AttributeHelper.AttributeCorrect(props.Single(x => x.Name == "ValueType"),5));
        }
    }
}