using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class Transaction
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public int GatewayId { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal Amount { get; set; }
    }
}
