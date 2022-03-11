using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class UnbilledTransit
    {
        public int Id { get; set; }
        public int PortalAccountId { get; set; }
        public DateTime Date { get; set; }
        public string VehiclePatent { get; set; } = null!;
        public decimal Amount { get; set; }
        public string? Direction { get; set; }
        public string? Gantry { get; set; }
        public string? DayType { get; set; }
        public string? Dealership { get; set; }
        public string? Axis { get; set; }
        public string? Place { get; set; }
        public string? RateType { get; set; }
        public string? Tag { get; set; }
        public decimal? Kilometres { get; set; }

        public virtual PortalAccount PortalAccount { get; set; } = null!;
    }
}
