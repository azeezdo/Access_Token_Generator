using System;
using System.Net;
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

		public async Task<CustomResponse> AuthenticateUser(User user, string password)
		{
			IdentityResult result = null;
            if (user is null)
            {
                return new CustomResponse { ResponseCode = 400, ResponseMessage= "Invalid username and/or password" };
            }
			else
			{
                var emailverify = await _userManager.FindByEmailAsync(user.Email);
                if (user != null)
                {
                    if (await _userManager.CheckPasswordAsync(user, password))
                    {
                        if (user.EmailConfirmed)
                        {
                            var response = new LoginResponse()
                            {
                           
                                    Email = user.Email,
                                    FullName = $"{user.FirstName + " " + user.LastName}",
                                    Id = user.Id,
                                    Token = await _token.GenerateTokenAsync(user)
                                
                            };
							return new CustomResponse { ResponseCode = 200, ResponseMessage = "login Successful", Data = response };
                           
                        }
                        throw new AccessViolationException("Kindly verify your email address to login");
                    }
                    throw new AccessViolationException("Invalid credentials");
                }
                throw new AccessViolationException("Invalid Credntials");
                
            }
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

