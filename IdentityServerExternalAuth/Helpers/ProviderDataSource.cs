using IdentityServerExternalAuth.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerExternalAuth.Helpers
{
    public class ProviderDataSource
    {
        public static IEnumerable<Provider> GetProviders()
        {
            return new List<Provider>
            {
                new Provider
                {
                    ProviderId = 1,
                    Name = "Facebook",
                    UserInfoEndPoint = "https://graph.facebook.com/v2.8/me"
                },
                new Provider
                {
                    ProviderId = 2,
                    Name = "Google",
                    UserInfoEndPoint = "https://www.googleapis.com/oauth2/v2/userinfo"
                },
                 new Provider
                {
                    ProviderId = 3,
                    Name = "Twitter",
                    UserInfoEndPoint = "https://api.twitter.com/1.1/account/verify_credentials.json"
                },
                 new Provider
                 {
                     ProviderId = 4,
                     Name="LinkedIn",
                     UserInfoEndPoint = "https://api.linkedin.com/v1/people/~:(id,email-address,first-name,last-name,location,industry,picture-url)?"
                 }
            };
        }
    }
}
