using System;
using System.Net;
using System.Text;
using AccessTokenDomain.Entity;
using AccessTokenDomain.Interfaces.IServices;
using AccessTokenDomain.Model.Request;
using AccessTokenDomain.Model.Response;
using AccessTokenInfrastructure.UnitofWorks;
using Azure;
using Microsoft.AspNetCore.Identity;
using static System.Net.WebRequestMethods;

namespace AccessTokenApplication.Services
{
	public class UserService : IUserService
	{
		private readonly IUnitOfWork _uow;
		private readonly UserManager<User> _userManager;
		private readonly ITokenGenerator _token;
		private readonly IEmailService _email;
		public UserService(IUnitOfWork uow, UserManager<User> userManager, ITokenGenerator token, IEmailService email)
		{
			_uow = uow;
			_userManager = userManager;
			_token = token;
			_email = email;

        }
        private const string AlphanumericChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public async Task<CustomResponse> CreateUser(UserRequest userdto)
		{
			User user = null;
			CustomResponse result = null;
			try
			{
				var userExist = await _uow.userRepo.GetByExpressionAsync(x => x.EmailAddress == user.EmailAddress);
				if(userExist == null)
				{
					var authuser = await _userManager.CreateAsync(user, userdto.Password);
					if(authuser.Succeeded)
					{
						var createuser = new User
						{
							FirstName = userdto.FirstName,
							LastName = userdto.LastName,
							EmailAddress = userdto.EmailAddress,
							DateCreated = DateTime.Now
						};

						_uow.userRepo.AddAsync(createuser);
						var rowchanges = await _uow.CompleteAsync();

						if(rowchanges > 0)
						{
                            var otp = GenerateOTP();
                            var msg = new EmailModel
                            {
                                EmailAddress = userdto.EmailAddress.Trim(),
                                FirstName = userdto.FirstName,
                                 OTP = otp,
                            
                            };
							await _email.SendVerificationEmail(msg);

                            return new CustomResponse { ResponseCode = 200, ResponseMessage = "user successfully created" };
						}
						else
						{
							return new CustomResponse { ResponseCode = 500, ResponseMessage = "Error occur while creating user" };
						}
					}
				}
				else
				{
					return new CustomResponse
					{
						ResponseCode = 400,
						ResponseMessage = $"Email address '{userdto.EmailAddress}' already taken."
					};
				}
			}
			catch (Exception ex)
			{

			}
			return result;
		}

		public async Task<CustomResponse> GenerateAccessToken(TokenRequestModel model)
		{
			CustomResponse res = null;
			try
			{
                // Ensure expiry is not in the past
                if (model.RequestedExpiry < DateTime.UtcNow)
                    throw new ArgumentException("Expiry date cannot be in the past.");

                // Cap expiry to 3 days from now
                DateTime maxAllowedExpiry = DateTime.UtcNow.AddDays(3);
                if (model.RequestedExpiry > maxAllowedExpiry)
                    throw new ArgumentException("Expiry date cannot be more than 3 days from now.");

                string token = GenerateRandomToken(6);
				var obj = new
				{
					Token = token,
					RequestExpiry = model.RequestedExpiry
				};
				return new CustomResponse { ResponseCode = 200, ResponseMessage = "token generated successfully", Data = obj };
            }
			catch (Exception ex)
			{

			}
			return res;
		}
        private string GenerateRandomToken(int length)
        {
            var random = new Random();
            var sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                var index = random.Next(AlphanumericChars.Length);
                sb.Append(AlphanumericChars[index]);
            }
            return sb.ToString();
        }

        private string GenerateOTP()
        {
            try
            {
                byte[] seed = Guid.NewGuid().ToByteArray();
                Random _random = new Random(BitConverter.ToInt32(seed, 0));
                int _rand = _random.Next(1000, 10000);

                return _rand.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}

