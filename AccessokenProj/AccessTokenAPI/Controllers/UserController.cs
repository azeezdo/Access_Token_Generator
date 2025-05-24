using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccessTokenDomain.Interfaces.IServices;
using AccessTokenDomain.Model.Request;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccessTokenAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _user;
        public UserController(IUserService user)
        {
            _user = user;
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
    }
}

