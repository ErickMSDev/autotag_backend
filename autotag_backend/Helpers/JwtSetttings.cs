using System;
namespace AutoTagBackEnd.Helpers
{
	public class JwtSettings
	{
		public string? SecretKey { get; set; }
		public string? AudienceToken { get; set; }
		public string? IssuerToken { get; set; }
		public string? ExpireMinutes { get; set; }
	}
}