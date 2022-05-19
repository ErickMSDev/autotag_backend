using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class GantryDirection
    {
        public GantryDirection()
        {
            Gantries = new HashSet<Gantry>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;

        public virtual ICollection<Gantry> Gantries { get; set; }
    }
}
