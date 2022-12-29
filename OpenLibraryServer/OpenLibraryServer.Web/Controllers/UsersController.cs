using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenLibraryServer.Models.DTOs;
using OpenLibraryServer.Service.Exceptions;
using OpenLibraryServer.Service.HelperFunctions;
using OpenLibraryServer.Service.Interfaces;

namespace OpenLibraryServer.Web.Controllers
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
            return await _userAuthService.Login(loginTo);
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterTO registerTo)
        {
            await _userAuthService.Register(registerTo);
            return CreatedAtAction(nameof(_userAuthService.Register), registerTo.UserName);
        }

        [HttpGet("{userId}")]
        public async Task<UserTO> GetById([FromRoute] string userId)
        {
            var guid = EntityHelpers.TryParseGuid(userId);
            return await _userAuthService.GetUserById(guid);
        }

        [Authorize]
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordTO passwordTo)
        {
            var userid = TokenHelpers.GetUserId(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _userAuthService.ChangeUserPassword(passwordTo, userid);
            return Ok();
        }

        [Authorize]
        [HttpPost("theme")]
        public async Task<IActionResult> ChangeTheme([FromBody] ChangeThemeTO changeThemeTo)
        {
            var userid = TokenHelpers.GetUserId(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _userAuthService.ChangeUserTheme(changeThemeTo, userid);
            return Ok();
        }
    }
}