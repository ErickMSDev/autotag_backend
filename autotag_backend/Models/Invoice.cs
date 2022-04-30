using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class Invoice
    {
        public Invoice()
        {
            InvoiceDetails = new HashSet<InvoiceDetail>();
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string? Code { get; set; }
        public int PurchaseOrderId { get; set; }
        public int InvoiceStateId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }

        public virtual InvoiceState InvoiceState { get; set; } = null!;
        public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
