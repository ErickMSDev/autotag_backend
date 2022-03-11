using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    /// <summary>
    /// Son las personas encargadas de los vehiculos
    /// </summary>
    public partial class Person
    {
        public Person()
        {
            VehicleAssignments = new HashSet<VehicleAssignment>();
        }

        public int Id { get; set; }
        public int AccountId { get; set; }
        public string? Run { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Mail { get; set; } = null!;
        public bool Enabled { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual ICollection<VehicleAssignment> VehicleAssignments { get; set; }
    }
}
