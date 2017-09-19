using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerExternalAuth.Entities
{
    public class ExternalUser 
    {
        public int Id { get; set; }
        public string Provider { get; set; }

        public string ExternalId { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
