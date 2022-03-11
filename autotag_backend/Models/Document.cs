using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class Document
    {
        public Document()
        {
            DocumentDetails = new HashSet<DocumentDetail>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public int PortalAccountId { get; set; }
        public int DocumentStateId { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public decimal? Amount { get; set; }
        public bool Downloaded { get; set; }
        public DateTime? PeriodStartDate { get; set; }
        public DateTime? PeriodEndDate { get; set; }
        public DateTime? DownloadDate { get; set; }
        public string? DteName { get; set; }

        public virtual DocumentState DocumentState { get; set; } = null!;
        public virtual PortalAccount PortalAccount { get; set; } = null!;
        public virtual ICollection<DocumentDetail> DocumentDetails { get; set; }
    }
}
