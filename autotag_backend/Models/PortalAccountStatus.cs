using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class PortalAccountStatus
    {
        public PortalAccountStatus()
        {
            PortalAccounts = new HashSet<PortalAccount>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<PortalAccount> PortalAccounts { get; set; }
    }
}
