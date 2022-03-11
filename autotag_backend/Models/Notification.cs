using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class Notification
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; } = null!;
        public int TypeId { get; set; }
        public bool Clicked { get; set; }
        public bool Removed { get; set; }
        public string? Link { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual NotificationType Type { get; set; } = null!;
    }
}
