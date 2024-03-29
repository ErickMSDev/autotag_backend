﻿using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class NotificationType
    {
        public NotificationType()
        {
            Notifications = new HashSet<Notification>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;

        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
