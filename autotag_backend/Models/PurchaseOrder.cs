using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class PurchaseOrder
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public DateTime CreationDate { get; set; }
        public int PurchaseOrderStateId { get; set; }
        public decimal? Amount { get; set; }
        public int? DiscountCodeId { get; set; }
        public decimal? AmountWithoutDiscount { get; set; }
    }
}
