using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutoTagBackEnd.Entities
{
	public class ConfirmEmailRequest
	{
		[Required]
		public string Token { get; set; }
	}
}

