using System;
namespace AutoTagBackEnd.Entities
{
	public class UserEmailOptions
	{
		public List<string> ToEmails { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public string Template { get; set; }
		public Dictionary<string, string> Params { get; set; }
	}
}

