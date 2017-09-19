using IdentityServerExternalAuth.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServerExternalAuth.Entities;
using Newtonsoft.Json.Linq;
using IdentityServerExternalAuth.Repositories.Interfaces;
using System.Net.Http;
using IdentityServerExternalAuth.Helpers;
using System.Net.Http.Headers;

namespace IdentityServerExternalAuth.Providers
{
    public class TwitterAuthProvider : ITwitterAuthProvider
    {
        private readonly IExternalUserRepository _externalUserRepository;
        private readonly IProviderRepository _providerRepository;
        private readonly HttpClient _httpClient;
        public TwitterAuthProvider(
             IExternalUserRepository externalUserRepository,
             IProviderRepository providerRepository,
             HttpClient httpClient
             )
        {
            _externalUserRepository = externalUserRepository;
            _providerRepository = providerRepository;
            _httpClient = httpClient;
        }
        public Provider Provider => _providerRepository.Get()
                                    .FirstOrDefault(x => x.Name.ToLower() == ProviderType.Twitter.ToString().ToLower());

        public JObject GetUserInfo(string accessToken)
        {
            if (Provider == null)
            {
                throw new ArgumentNullException(nameof(Provider));
            }

            var request = new Dictionary<string, string>();
            request.Add("tokenString", accessToken);
            request.Add("endpoint", Provider.UserInfoEndPoint);

            var authorizationHeaderParams = QueryBuilder.GetQuery(request, ProviderType.Twitter);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", authorizationHeaderParams);

            var result = _httpClient.GetAsync(Provider.UserInfoEndPoint).Result;

            if (result.IsSuccessStatusCode)
            {
                var infoObject = JObject.Parse(result.Content.ReadAsStringAsync().Result);
                return infoObject;
            }
            return null;
        }
    }
}
