﻿using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class Role
    {
        public Role()
        {
            Accounts = new HashSet<Account>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
