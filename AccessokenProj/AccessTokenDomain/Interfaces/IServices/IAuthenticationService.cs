using System;
using AccessTokenDomain.Entity;
using AccessTokenDomain.Model.Response;

namespace AccessTokenDomain.Interfaces.IServices
{
	public interface IAuthenticationService
	{
        Task<CustomResponse> ValidateOTP(string otp, string email);
        Task<CustomResponse> AuthenticateUser(User user, string password);

    }
}

