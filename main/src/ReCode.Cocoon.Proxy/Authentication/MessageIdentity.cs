using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MessagePack;

namespace ReCode.Cocoon.Proxy.Authentication
{
    [MessagePackObject]
    public class MessageIdentity
    {
        [Key(0)]
        public string AuthenticationType { get; set; }

        [Key(1)]
        public bool IsAuthenticated { get; set; }

        [Key(2)]
        public string Label { get; set; }

        [Key(3)]
        public string NameClaimType { get; set; }

        [Key(4)]
        public string RoleClaimType { get; set; }

        [Key(5)]
        public List<MessageClaim> Claims { get; set; }

        public ClaimsIdentity ToClaimsIdentity()
        {
            var claims = Claims.Select(c => c.ToClaim());
            return new ClaimsIdentity(claims,
                AuthenticationType, NameClaimType, RoleClaimType);
        }
    }
}