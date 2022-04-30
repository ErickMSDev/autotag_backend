using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class Transaction
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int GatewayId { get; set; }
        public int? GatewayOrder { get; set; }
        public string? GatewayToken { get; set; }
        public int? GatewayPaymentMethod { get; set; }
        public int TransactionStateId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public bool IsDevelopment { get; set; }

        public virtual Gateway Gateway { get; set; } = null!;
        public virtual Invoice Invoice { get; set; } = null!;
        public virtual TransactionState TransactionState { get; set; } = null!;
    }
}
