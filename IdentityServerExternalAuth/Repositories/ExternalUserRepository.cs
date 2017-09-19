using IdentityServerExternalAuth.Data;
using IdentityServerExternalAuth.Entities;
using IdentityServerExternalAuth.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerExternalAuth.Repositories
{
   
   
    public class ExternalUserRepository : IExternalUserRepository
    {
        private readonly DatabaseContext _dbContext;
        private readonly DbSet<ExternalUser> _dbSet;
        public ExternalUserRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<ExternalUser>();
        }

        public void Add(ExternalUser user)
        {
            _dbSet.Add(user);
            _dbContext.SaveChanges();
        }

        public  IEnumerable<ExternalUser> Get()
        {
            return _dbSet.AsEnumerable();
        }
    }
}
