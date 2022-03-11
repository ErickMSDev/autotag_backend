using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class VehicleAssignment
    {
        public int Id { get; set; }
        public int? PersonId { get; set; }
        public string VehiclePatent { get; set; } = null!;
        public DateTime AssignmentDate { get; set; }

        public virtual Person? Person { get; set; }
    }
}
