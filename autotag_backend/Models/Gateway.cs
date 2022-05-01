using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class Gateway
    {
        public Gateway()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool Enabled { get; set; }
        public bool UseDevelopmentData { get; set; }
        public string? ApiKeyProd { get; set; }
        public string? ApiKeyDev { get; set; }
        public string? SecretKeyProd { get; set; }
        public string? SecretKeyDev { get; set; }
        public string? DebtCollectorIdProd { get; set; }
        public string? DebtCollectorIdDev { get; set; }
        public string? RestApiUrlProd { get; set; }
        public string? RestApiUrlDev { get; set; }
        public int? PaymentMethod { get; set; }
        public string? UrlConfirmation { get; set; }
        public string? BackendUrlReturn { get; set; }
        public string? UrlReturn { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
