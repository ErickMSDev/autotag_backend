using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class PatentGroup
    {
        public PatentGroup()
        {
            Vehicles = new HashSet<Vehicle>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string PatentDigits { get; set; } = null!;
        public string? LatestPatentDownloaded { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}
