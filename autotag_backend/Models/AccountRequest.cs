using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class AccountRequest
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Code { get; set; } = null!;
    }
}
