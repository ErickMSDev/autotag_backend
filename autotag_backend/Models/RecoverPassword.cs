using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class RecoverPassword
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public string Code { get; set; } = null!;
    }
}
