using System;
using AccessTokenDomain.Model.Request;
using AccessTokenDomain.Model.Response;

namespace AccessTokenDomain.Interfaces.IServices
{
	public interface IUserService
	{
        Task<CustomResponse> CreateUser(UserRequest userdto);
        Task<CustomResponse> GenerateAccessToken(TokenRequestModel model);

    }
}

