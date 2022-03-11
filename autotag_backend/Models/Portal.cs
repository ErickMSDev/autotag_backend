using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class Portal
    {
        public Portal()
        {
            PortalAccounts = new HashSet<PortalAccount>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Order { get; set; }

        public virtual ICollection<PortalAccount> PortalAccounts { get; set; }
    }
}
