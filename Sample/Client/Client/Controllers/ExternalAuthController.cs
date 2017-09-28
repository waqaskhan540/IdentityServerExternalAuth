using Client.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Controllers
{
    [Route("[controller]/[action]")]
    public class ExternalAuthController : Controller
    {
        private const string Authority = "http://localhost:17640/";
        private const string ClientId = "Test.Client";
        private const string Secret = "secret";
        private const string ApiBaseAddress = "http://localhost:4343/";

        [HttpPost]
        public async Task<JsonResult> Login(ExternalUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var disco = await DiscoveryClient.GetAsync(Authority);
                if (disco.IsError) throw new Exception(disco.Error);

                var tokenClient = new TokenClient(disco.TokenEndpoint, ClientId, Secret);

                var payload = new
                {
                    // provider can be [facebook, google , twitter]
                    provider = model.Provider,

                    //retrieve access token from your respective provider
                    //in case of facebook , use facebook javascript SDK or C# SDK
                    external_token = model.AccessToken,

                    //email is optional, because it may or maynot be retrieved from
                    //the provider, if email is retrieved from the provider then a new user
                    //is created using that email, else the extension grant sends the user object
                    // and prompts to provide an email in order to create a user, this would happen for the
                    // first time only, every subsequent request (after user creation) would only require the
                    //respective access token from the provider.
                    email = model.Email
                };

                //Use our custom ExtensionGrant i.e. "external"

                var result = await tokenClient.RequestCustomGrantAsync("external", "Test.WebApi", payload);

                /*
                 * 
                 * if there is an Error in the result then it means the user has not yet been created, so
                 * the user object is returned and user is prompted to provide an email 
                 * 
                 * */
                if(result.IsError) return new JsonResult(result.Json);


                /*
                 * 
                 * If we get the access_token back from the Identity server then it means
                 * use is Logged In successfully. 
                 * So make an API request to our protected API.
                 * 
                 * */

                if (!string.IsNullOrWhiteSpace(result.AccessToken))
                {
                    return new JsonResult(new { access_token = result.AccessToken });
                }
               

            }
            return new JsonResult(null);
        }

        private async Task<JsonResult> ApiRequest(string accessToken)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(ApiBaseAddress)
            };

            client.SetBearerToken(accessToken);

            var result = await client.GetStringAsync("Values/Get");
            return new JsonResult(JArray.Parse(result));
        }
    }
}
