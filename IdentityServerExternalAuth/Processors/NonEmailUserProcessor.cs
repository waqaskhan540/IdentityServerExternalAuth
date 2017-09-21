using IdentityServerExternalAuth.Interfaces.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Validation;
using IdentityServerExternalAuth.Repositories.Interfaces;
using IdentityServerExternalAuth.Entities;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using IdentityServer4.Models;

namespace IdentityServerExternalAuth.Processors
{
    public class NonEmailUserProcessor : INonEmailUserProcessor
    {
        private readonly IExternalUserRepository _externalUserRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public NonEmailUserProcessor(
            IExternalUserRepository externalUserRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            _externalUserRepository = externalUserRepository ?? throw new ArgumentNullException(nameof(externalUserRepository));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        public GrantValidationResult Process(JObject userInfo,string provider)
        {

            var userEmail = userInfo.Value<string>("email");

            if (provider.ToLower() == "linkedin")
                userEmail = userInfo.Value<string>("emailAddress");

            var userExternalId = userInfo.Value<string>("id");

            if (userEmail == null)
            {
                var registeredUser = _externalUserRepository.Get().FirstOrDefault(x => x.ExternalId == userExternalId);
                if (registeredUser == null)
                {
                    var customResponse = new Dictionary<string, object>();
                    customResponse.Add("userInfo", userInfo);

                    
                    return new GrantValidationResult(TokenRequestErrors.InvalidRequest, "could not retrieve user's email from the given provider, include email paramater and send request again.", customResponse);
                    
                }
                else
                {
                    var existingUser =  _userManager.FindByIdAsync(registeredUser.UserId).Result;
                    var userClaims = _userManager.GetClaimsAsync(existingUser).Result;
                    return new GrantValidationResult(existingUser.Id, provider, userClaims, provider, null);
                }

            }
            else
            {
                var new_user = new ApplicationUser { Email = userEmail,UserName = userEmail };
                var result =  _userManager.CreateAsync(new_user).Result;
                if (result.Succeeded)
                {
                    _externalUserRepository.Add(new ExternalUser { ExternalId = userExternalId, Provider = provider, UserId = new_user.Id });
                    var userClaims = _userManager.GetClaimsAsync(new_user).Result;
                    return new GrantValidationResult(new_user.Id, provider, userClaims, provider, null);
                }
                return new GrantValidationResult(TokenRequestErrors.InvalidRequest, "user could not be created, please try again");
            }

        }
    }
}
