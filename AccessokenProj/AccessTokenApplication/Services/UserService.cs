using System;
using AccessTokenDomain.Entity;
using AccessTokenDomain.Interfaces.IServices;
using AccessTokenDomain.Model.Request;
using AccessTokenDomain.Model.Response;
using AccessTokenInfrastructure.UnitofWorks;
using Microsoft.AspNetCore.Identity;

namespace AccessTokenApplication.Services
{
	public class UserService : IUserService
	{
		private readonly IUnitOfWork _uow;
		private readonly UserManager<User> _userManager;
		public UserService(IUnitOfWork uow, UserManager<User> userManager)
		{
			_uow = uow;
			_userManager = userManager;
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
	}
}

