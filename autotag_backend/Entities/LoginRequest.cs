using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutoTagBackEnd.Entities
{
	public class LoginRequest
	{
		[Required]
		//[DefaultValue("erick.ms.dev@gmail.com")]
		public string? Email { get; set; }

		[Required]
		//[DefaultValue("EM.,2021")]
		public string? Password { get; set; }
	}
}

