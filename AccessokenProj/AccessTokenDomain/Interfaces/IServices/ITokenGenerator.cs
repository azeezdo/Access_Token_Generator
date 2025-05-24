using System;
using AccessTokenDomain.Entity;

namespace AccessTokenDomain.Interfaces.IServices
{
	public interface ITokenGenerator
	{
        Task<string> GenerateTokenAsync(User user);

    }
}

