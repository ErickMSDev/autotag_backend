using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class DiscountCode
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public bool IsRecurring { get; set; }
        public int ProductId { get; set; }
        public int? PaymentCycleId { get; set; }
        public int DiscountCodeType { get; set; }
        public decimal Value { get; set; }
        public int? MaximumUses { get; set; }
        public bool OnlyNewCustomers { get; set; }
    }
}
