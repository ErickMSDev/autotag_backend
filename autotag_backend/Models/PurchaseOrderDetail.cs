using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class PurchaseOrderDetail
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public int ProductId { get; set; }
        public int? PaymentCycleId { get; set; }
        public decimal Amount { get; set; }
    }
}
