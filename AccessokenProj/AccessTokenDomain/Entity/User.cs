using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AccessTokenDomain.Entity
{
	public class User : IdentityUser
	{
		
		public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DateCreated { get; set; }
        public string ModifyBy { get; set; }
        public DateTime Modify { get; set; }
        public string? OTP { get; set; }
        public DateTime? OtpSubmittedTime { get; set; }
        public string? AccessToken { get; set; }
        public DateTime RequestExpiry { get; set; }


    }
}

