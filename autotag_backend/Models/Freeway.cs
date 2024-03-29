﻿using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class Freeway
    {
        public Freeway()
        {
            Gantries = new HashSet<Gantry>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public int PortalId { get; set; }
        public string? ExternalCode { get; set; }
        public string Name { get; set; } = null!;
        public int Order { get; set; }

        public virtual Portal Portal { get; set; } = null!;
        public virtual ICollection<Gantry> Gantries { get; set; }
    }
}
