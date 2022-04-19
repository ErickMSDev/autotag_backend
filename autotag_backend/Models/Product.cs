using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool Enabled { get; set; }
        public bool IsComplement { get; set; }
        public int? ParentProductId { get; set; }
        public int? Order { get; set; }
    }
}
