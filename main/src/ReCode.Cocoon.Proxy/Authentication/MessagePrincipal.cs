using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MessagePack;

namespace ReCode.Cocoon.Proxy.Authentication
{
    [MessagePackObject]
    public class MessagePrincipal
    {
        [Key(0)]
        public List<MessageIdentity>? Identities { get; set; }

        public ClaimsPrincipal ToClaimsPrincipal()
        {
            if (Identities is null) return new ClaimsPrincipal();
            
            var identities =
                Identities.Select(i => i.ToClaimsIdentity());
            return new ClaimsPrincipal(identities);
        }
    }
}