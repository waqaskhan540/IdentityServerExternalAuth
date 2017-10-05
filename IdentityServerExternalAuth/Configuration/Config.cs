using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServerExternalAuth.Configuration
{
    public class Config
    {
        // scopes define the resources in your systems

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityServer4.Models.IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("Test.WebApi","Test WebApi")
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "Test.Client",
                    ClientName = "LDSCore",
                    AllowedGrantTypes = new[] {GrantType.ResourceOwnerPassword,"external"},
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {
                        "Test.WebApi",
                        StandardScopes.Email,
                        StandardScopes.OpenId,
                        StandardScopes.Profile
                    },
                    AccessTokenType = AccessTokenType.Jwt,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AccessTokenLifetime = 86400,
                    AllowOfflineAccess = true,
                    IdentityTokenLifetime = 86400,
                    AlwaysSendClientClaims = true,
                    Enabled = true,
                }
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password",
                    Claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Role, "Admin"),
                        new Claim(JwtClaimTypes.Name, "jwtName")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password"
                }
            };
        }
    }
}
