using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using JetBrains.Annotations;
using MessagePack;
#pragma warning disable CS8618

namespace ReCode.Cocoon.Proxy.Authentication
{
    [MessagePackObject]
    [PublicAPI]
    public class MessagePrincipal
    {
        [Key(0)]
        public List<MessageIdentity> Identities { get; set; }

        public ClaimsPrincipal ToClaimsPrincipal()
        {
            var identities =
                Identities.Select(i => i.ToClaimsIdentity());
            return new ClaimsPrincipal(identities);
        }
    }
}