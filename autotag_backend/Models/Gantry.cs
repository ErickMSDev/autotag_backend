using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class Gantry
    {
        public int Id { get; set; }
        public int FreewayId { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Lat { get; set; } = null!;
        public string Lon { get; set; } = null!;
        public int GantryDirectionId { get; set; }
        public string? PrevCode { get; set; }
        public double? PrevDistance { get; set; }

        public virtual Freeway Freeway { get; set; } = null!;
        public virtual GantryDirection GantryDirection { get; set; } = null!;
    }
}
