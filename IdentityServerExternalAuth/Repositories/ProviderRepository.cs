using IdentityServerExternalAuth.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServerExternalAuth.Entities;
using IdentityServerExternalAuth.Helpers;

namespace IdentityServerExternalAuth.Repositories
{
    public class ProviderRepository : IProviderRepository
    {

        public  IEnumerable<Provider> Get()
        {
            return ProviderDataSource.GetProviders();
        }
    }
}
