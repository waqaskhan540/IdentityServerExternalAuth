using IdentityServer4.Validation;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerExternalAuth.Interfaces.Processors
{
    public interface INonEmailUserProcessor
    {
        GrantValidationResult Process(JObject userInfo,string provider);
    }
}
