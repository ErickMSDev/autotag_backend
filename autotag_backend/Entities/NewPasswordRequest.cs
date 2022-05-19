using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutoTagBackEnd.Entities
{
	public class NewPasswordRequest
	{
		[Required]
		public string Token { get; set; }
		public string Password { get; set; }
	}
}

