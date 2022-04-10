using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class DomainBlacklist
    {
        public int Id { get; set; }
        public string DomainName { get; set; } = null!;
    }
}
