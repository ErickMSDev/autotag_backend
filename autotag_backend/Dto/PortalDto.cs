using System;
namespace AutoTagBackEnd.Dto
{
	public class PortalDto
	{
		public PortalDto()
		{
		}

		public int Id { get; set; }
		public string Code { get; set; } = null!;
		public string Name { get; set; } = null!;
		public int Order { get; set; }
	}
}

