using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class PeopleTransit
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string VehiclePatent { get; set; } = null!;
        public DateTime StartTransit { get; set; }
        public DateTime EndTransit { get; set; }

        public virtual Person Person { get; set; } = null!;
    }
}
