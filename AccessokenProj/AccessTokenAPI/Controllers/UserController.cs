using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AccessTokenDomain.Entity;
using AccessTokenDomain.Interfaces.IServices;
using AccessTokenDomain.Model.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccessTokenAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _user;
        private readonly IAuthenticationService _authenticationService;
        public UserController(IUserService user, IAuthenticationService authenticationService)
        {
            _user = user;
            _authenticationService = authenticationService;
        }


        [HttpPost("createUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest request)
        {
            var result = await _user.CreateUser(request);
            if(result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("validateotp")]
        public async Task<IActionResult> ValidateOtp([FromQuery] string otp, string email)
        {
            var response = await _authenticationService.ValidateOTP(otp, email.Trim());
            if (response.ResponseCode == (int)HttpStatusCode.OK)
            {
                return Ok(response);
            }
            else if (response.ResponseCode == (int)HttpStatusCode.NotFound)
            {
                return NotFound(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(User user, string password)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _authenticationService.AuthenticateUser(user, password);
                if (result != null)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }


            catch (AccessViolationException ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

    }
}

