using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class TransactionState
    {
        public TransactionState()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
