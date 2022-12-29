using AudioServer.Models.DTOs;
using AudioServer.Service.HelperFunctions;
using AudioServer.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
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

            return Ok();
        }

        [HttpGet("{userId}")]
        public async Task<UserTO> GetById([FromRoute] string userId)
        {
            var guid = EntityHelpers.TryParseGuid(userId);

            return await _userAuthService.GetUserById(guid);
        }
    }
}