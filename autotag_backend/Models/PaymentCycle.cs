using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class PaymentCycle
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
