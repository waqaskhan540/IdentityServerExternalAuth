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
    public class NonEmailUserProcessor<TUser> : INonEmailUserProcessor where TUser : IdentityUser , new()
    {
       // private readonly IExternalUserRepository _externalUserRepository;
        private readonly UserManager<TUser> _userManager;
        public NonEmailUserProcessor(
         //   IExternalUserRepository externalUserRepository,
            UserManager<TUser> userManager
            )
        {
          //  _externalUserRepository = externalUserRepository ?? throw new ArgumentNullException(nameof(externalUserRepository));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        public async Task<GrantValidationResult> ProcessAsync(JObject userInfo,string provider)
        {

            var userEmail = userInfo.Value<string>("email");

            if (provider.ToLower() == "linkedin")
                userEmail = userInfo.Value<string>("emailAddress");

            var userExternalId = userInfo.Value<string>("id");

            if (userEmail == null)
            {
                var existingUser = await _userManager.FindByLoginAsync(provider, userExternalId);
                if (existingUser == null)
                {
                    var customResponse = new Dictionary<string, object>();
                    customResponse.Add("userInfo", userInfo);
                     return new GrantValidationResult(TokenRequestErrors.InvalidRequest, "could not retrieve user's email from the given provider, include email paramater and send request again.", customResponse);
                    
                }
                else
                {
                     existingUser = await _userManager.FindByIdAsync(existingUser.Id);
                    var userClaims = await _userManager.GetClaimsAsync(existingUser);
                    return new GrantValidationResult(existingUser.Id, provider, userClaims, provider, null);
                }

            }
            else
            {
                var newUser = new TUser { Email = userEmail,UserName = userEmail };
                var result = await _userManager.CreateAsync(newUser);
                if (result.Succeeded)
                {                   
                    await _userManager.AddLoginAsync(newUser, new UserLoginInfo(provider, userExternalId, provider));
                    var userClaims = await _userManager.GetClaimsAsync(newUser);
                    return new GrantValidationResult(newUser.Id, provider, userClaims, provider, null);
                }
                return new GrantValidationResult(TokenRequestErrors.InvalidRequest, "user could not be created, please try again");
            }

        }
    }
}
