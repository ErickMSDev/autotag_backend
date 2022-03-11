using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class Account
    {
        public Account()
        {
            Notifications = new HashSet<Notification>();
            People = new HashSet<Person>();
            PortalAccounts = new HashSet<PortalAccount>();
        }

        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public bool Enabled { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Person> People { get; set; }
        public virtual ICollection<PortalAccount> PortalAccounts { get; set; }
    }
}
