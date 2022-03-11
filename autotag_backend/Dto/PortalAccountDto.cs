using System;
namespace AutoTagBackEnd.Dto
{
	public class PortalAccountDto
	{
        public int Id { get; set; }
        public int PortalId { get; set; }
        public string PortalCode { get; set; } = null!;
        public string PortalName { get; set; } = null!;
        public string Run { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool Enabled { get; set; }
    }
}

