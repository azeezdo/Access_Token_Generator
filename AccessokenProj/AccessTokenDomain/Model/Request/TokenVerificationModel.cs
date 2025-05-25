using System;
namespace AccessTokenDomain.Model.Request
{
	public class TokenVerificationModel
	{
        public string Token { get; set; }
        public string CurrentUserId { get; set; }
    }
}

