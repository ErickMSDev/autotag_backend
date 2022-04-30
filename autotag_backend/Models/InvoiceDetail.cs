using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class InvoiceDetail
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string Description { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal? Amount { get; set; }

        public virtual Invoice Invoice { get; set; } = null!;
    }
}
