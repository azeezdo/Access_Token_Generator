using System;
using AccessTokenDomain.Model.Request;
using AccessTokenDomain.Model.Response;

namespace AccessTokenDomain.Interfaces.IServices
{
	public interface IEmailService
	{
        Task<CustomResponse> SendVerificationEmail(EmailModel model);
    }
}

