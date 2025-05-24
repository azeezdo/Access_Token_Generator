using System;
namespace AccessTokenDomain.Model.Request
{
	public class TokenRequestModel
	{
		public string Token { get; set; }
        public DateTime Expiry { get; set; }
		public DateTime RequestedExpiry { get; set; }
	}
}

