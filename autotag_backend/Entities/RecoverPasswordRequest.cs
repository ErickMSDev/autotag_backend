using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutoTagBackEnd.Entities
{
	public class RecoverPasswordRequest
	{
		[Required]
		public string Email { get; set; }
	}
}

