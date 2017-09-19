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
    public class EmailUserProcessor : IEmailUserProcessor
    {
        private readonly IExternalUserRepository _externalUserRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public EmailUserProcessor(
            IExternalUserRepository externalUserRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            _externalUserRepository = externalUserRepository ?? throw new ArgumentNullException(nameof(externalUserRepository));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        public GrantValidationResult Process(JObject userInfo,string email,string provider)
        {
            var userEmail = email;
            var userExternalId = userInfo.Value<string>("id");

           

            if (string.IsNullOrWhiteSpace(userExternalId))
            {
                return new GrantValidationResult(TokenRequestErrors.InvalidRequest, "could not retrieve user Id from the token provided");
            }

            var new_user = new ApplicationUser { Email = userEmail ,UserName = userEmail};
            var result =  _userManager.CreateAsync(new_user).Result;
            if (result.Succeeded)
            {
                _externalUserRepository.Add(new ExternalUser { ExternalId = userExternalId, Provider = provider, UserId = new_user.Id });
                var userClaims = _userManager.GetClaimsAsync(new_user).Result;
                return new GrantValidationResult(new_user.Id, provider, userClaims, provider, null);
            }
            return new GrantValidationResult(TokenRequestErrors.InvalidRequest, "could not create user , please try again.");
        }
    }
}
