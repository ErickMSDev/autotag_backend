using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class Gateway
    {
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
    }
}
