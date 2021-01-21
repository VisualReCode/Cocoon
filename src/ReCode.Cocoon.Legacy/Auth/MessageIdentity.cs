using System.Collections.Generic;
using MessagePack;

namespace ReCode.Cocoon.Legacy.Auth
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
    }
}