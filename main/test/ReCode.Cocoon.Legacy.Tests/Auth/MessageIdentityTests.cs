using System.Linq;
using System.Reflection;
using MessagePack;
using ReCode.Cocoon.Legacy.Auth;
using Xunit;

namespace ReCode.Cocoon.Legacy.Tests.Auth
{
    public class MessageIdentityTests
    {
        [Fact]
        public void AttributesAndOrderCorrectlyApplied()
        {
            var props = typeof(MessageIdentity).GetProperties();
            Assert.True(AttributeHelper.AttributeCorrect(props.Single(x => x.Name == "AuthenticationType"),0));
            Assert.True(AttributeHelper.AttributeCorrect(props.Single(x => x.Name == "IsAuthenticated"),1));
            Assert.True(AttributeHelper.AttributeCorrect(props.Single(x => x.Name == "Label"),2));
            Assert.True(AttributeHelper.AttributeCorrect(props.Single(x => x.Name == "NameClaimType"),3));
            Assert.True(AttributeHelper.AttributeCorrect(props.Single(x => x.Name == "RoleClaimType"),4));
            Assert.True(AttributeHelper.AttributeCorrect(props.Single(x => x.Name == "Claims"),5));
        }
    }
}