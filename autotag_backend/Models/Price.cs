using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class Price
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int? PaymentCycleId { get; set; }
        public decimal Amount { get; set; }
        public string? DiscountText { get; set; }
        public bool Enabled { get; set; }
    }
}
