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
        public bool Enabled { get; set; }
        public bool Removed { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? DeletionDate { get; set; }
        public bool IsBeingProcessed { get; set; }
        public bool HasError { get; set; }
        public string? ErrorMessage { get; set; }
        public byte[] RowVersion { get; set; } = null!;
        public bool HasPendingProcess { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual Portal Portal { get; set; } = null!;
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<UnbilledTransit> UnbilledTransits { get; set; }
    }
}
