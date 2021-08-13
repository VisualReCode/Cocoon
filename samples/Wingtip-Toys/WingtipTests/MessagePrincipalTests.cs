using System.Collections.Generic;
using MessagePack;
using ReCode.Cocoon.Legacy.Auth;
using Xunit;

namespace WingtipTests
{
    public class MessagePrincipalTests
    {
        [Fact]
        public void SerializesAndDeserializes()
        {
            var source = new MessagePrincipal
            {
                Identities = new List<MessageIdentity>()
            };
            var sourceIdentity = new MessageIdentity
            {
                Claims = new List<MessageClaim>()
            };
            var sourceClaim = new MessageClaim
            {
                Type = "Foo",
                Value = "Bar"
            };
            sourceIdentity.Claims.Add(sourceClaim);
            source.Identities.Add(sourceIdentity);

            var bytes = MessagePackSerializer.Serialize(source);

            var actual = MessagePackSerializer.Deserialize<MessagePrincipal>(bytes);

            Assert.Single(actual.Identities);
            Assert.Single(actual.Identities[0].Claims);
            Assert.Equal("Foo", actual.Identities[0].Claims[0].Type);
            Assert.Equal("Bar", actual.Identities[0].Claims[0].Value);
        }
    }
}
