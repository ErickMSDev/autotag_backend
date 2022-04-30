using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class InvoiceState
    {
        public InvoiceState()
        {
            Invoices = new HashSet<Invoice>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;

        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
