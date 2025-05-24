using System;
namespace AccessTokenDomain.Model.Response
{
	public class LoginResponse
	{
        public string Id { get; set; }
        public string Token { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}

