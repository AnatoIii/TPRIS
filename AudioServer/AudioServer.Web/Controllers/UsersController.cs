using AudioServer.Models.DTOs;
using AudioServer.Service.HelperFunctions;
using AudioServer.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace AudioServer.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserAuthService _userAuthService;

        public UsersController(IUserAuthService userAuthService)
        {
            _userAuthService = userAuthService;
        }

        [HttpPost("login")]
        public async Task<TokenTO> Login([FromBody] LoginTO loginTo)
        {
            TokenTO result = await _userAuthService.Login(loginTo);

            return result;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterTO registerTo)
        {
            await _userAuthService.Register(registerTo);

            return Ok(registerTo.UserName);
        }

        [HttpGet]
        public async Task<UserTO> GetById()
        {
            //var token = Request.Headers["authorization"][0];
            //var handler = new JwtSecurityTokenHandler();
            //var jwtSecurityToken = handler.ReadJwtToken(token);

            //var guid = jwtSecurityToken.Claims.ToList()[0];
            var guid = HttpContext.User.Identities.First().Claims.ToList()[0].Value;
            return await _userAuthService.GetUserById(Guid.Parse(guid));
        }
    }
}