using System;
using Microsoft.AspNetCore.Identity;

namespace AccessTokenDomain.Entity
{
	public class User : IdentityUser
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DateCreated { get; set; }
        public string ModifyBy { get; set; }
        public DateTime Modify { get; set; }

    }
}

