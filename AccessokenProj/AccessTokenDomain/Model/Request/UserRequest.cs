using System;
namespace AccessTokenDomain.Model.Request
{
	public class UserRequest
	{
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}

