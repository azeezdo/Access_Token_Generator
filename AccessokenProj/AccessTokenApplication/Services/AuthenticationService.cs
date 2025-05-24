using System;
using System.Net;
using AccessTokenDomain.Entity;
using AccessTokenDomain.Interfaces.IServices;
using AccessTokenDomain.Model.Response;
using AccessTokenInfrastructure.UnitofWorks;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

namespace AccessTokenApplication.Services
{
	public class AuthenticationService : IAuthenticationService
	{
        private readonly IUnitOfWork _uow;
        private readonly UserManager<User> _userManager;
        private readonly ITokenGenerator _token;
        public AuthenticationService(ITokenGenerator token, UserManager<User> userManager, IUnitOfWork uow)
		{
            _userManager = userManager;
            _token = token;
            _uow = uow;
        }

        public async Task<CustomResponse> AuthenticateUser(User user, string password)
        {
            IdentityResult result = null;
            if (user is null)
            {
                return new CustomResponse { ResponseCode = 400, ResponseMessage = "Invalid username and/or password" };
            }
            else
            {
                var emailverify = await _userManager.FindByEmailAsync(user.Email);
                if (emailverify != null)
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

        public async Task<CustomResponse> ValidateOTP(string otp, string email)
        {
            CustomResponse res = null;
            try
            {

                var verifyUser = await _userManager.FindByEmailAsync(email.Trim().ToLower());
                if (verifyUser != null)
                {
                    var user = await _uow.userRepo.GetByExpressionAsync(x => x.EmailAddress == email);
                    TimeSpan ts = DateTime.Now.TimeOfDay.Subtract(user.OtpSubmittedTime.GetValueOrDefault().TimeOfDay);
                    if (user.OTP == otp && ts.TotalSeconds <= 600)
                    {
                        user.EmailConfirmed = true;
                       

                        var result = await _userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            return new CustomResponse{ResponseCode = 200, ResponseMessage = "OTP validation succesful"};
                        }

                    }
                    else
                    {
                        return new CustomResponse { ResponseCode = 400, ResponseMessage = "Wrong OTP! Please provide valid OTP received in your email" };
                    }
                }
                else
                {
                    return new CustomResponse { ResponseCode = 400, ResponseMessage = "Email does not exist" };

                }
            }
            catch (Exception ex)
            {

                return new CustomResponse { ResponseCode = 400, ResponseMessage = "OTP Validation Failed" };
            }
            return res;
        }
    }
}

