using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class PortalAccount
    {
        public PortalAccount()
        {
            Documents = new HashSet<Document>();
            UnbilledTransits = new HashSet<UnbilledTransit>();
        }

        public int Id { get; set; }
        public int AccountId { get; set; }
        public int PortalId { get; set; }
        public string Run { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int PortalAccountStatusId { get; set; }
        public bool Removed { get; set; }
        public bool HasPendingProcess { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? DeletionDate { get; set; }
        public bool HasFirstSuccessfulProcess { get; set; }
        public string? ErrorMessage { get; set; }
        public bool ErrorMailAlreadySent { get; set; }
        public byte[] RowVersion { get; set; } = null!;

        public virtual Account Account { get; set; } = null!;
        public virtual Portal Portal { get; set; } = null!;
        public virtual PortalAccountStatus PortalAccountStatus { get; set; } = null!;
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<UnbilledTransit> UnbilledTransits { get; set; }
    }
}
