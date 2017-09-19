using IdentityServerExternalAuth.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerExternalAuth.Repositories.Interfaces
{
    public interface IProviderRepository
    {
        IEnumerable<Provider> Get();
        

    }
}
