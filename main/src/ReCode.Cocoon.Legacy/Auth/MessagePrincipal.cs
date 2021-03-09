using System.Collections.Generic;
using System.Security.Claims;
using MessagePack;

namespace ReCode.Cocoon.Legacy.Auth
{
    [MessagePackObject]
    public class MessagePrincipal
    {
        [Key(0)]
        public List<MessageIdentity> Identities { get; set; }

        public static MessagePrincipal FromClaimsPrincipal(ClaimsPrincipal source)
        {
            var messagePrincipal = new MessagePrincipal
            {
                Identities = new List<MessageIdentity>(),
            };

            foreach (var sourceIdentity in source.Identities)
            {
                var messageIdentity = new MessageIdentity
                {
                    AuthenticationType = sourceIdentity.AuthenticationType,
                    IsAuthenticated = sourceIdentity.IsAuthenticated,
                    Label = sourceIdentity.Label,
                    NameClaimType = sourceIdentity.NameClaimType,
                    RoleClaimType = sourceIdentity.RoleClaimType,
                    Claims = new List<MessageClaim>(),
                };

                foreach (var sourceClaim in sourceIdentity.Claims)
                {
                    var messageClaim = new MessageClaim
                    {
                        Issuer = sourceClaim.Issuer,
                        OriginalIssuer = sourceClaim.OriginalIssuer,
                        Properties = sourceClaim.Properties,
                        Type = sourceClaim.Type,
                        Value = sourceClaim.Value,
                        ValueType = sourceClaim.ValueType
                    };
                    messageIdentity.Claims.Add(messageClaim);
                }

                messagePrincipal.Identities.Add(messageIdentity);
            }

            return messagePrincipal;
        }
    }
}