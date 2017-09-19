using IdentityServerExternalAuth.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerExternalAuth.Repositories.Interfaces
{
    public interface IExternalUserRepository
    {
        IEnumerable<ExternalUser> Get();
        void Add(ExternalUser user);
    }
}
