using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutoTagBackEnd.Entities
{
	public class RegisterRequest
	{
		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		[Required]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }
	}
}

