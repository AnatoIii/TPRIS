using System.Collections.Generic;
using System.Linq;
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
    public class ChatsController : Controller
    {
        private readonly IChatService _chatService;

        public ChatsController(IChatService chatService)
        {
            _chatService = chatService;
        }
        
        [HttpGet("{chatId}")]
        public async Task<ChatTO> GetById([FromRoute] string chatId)
        {
            var guid = EntityHelpers.TryParseGuid(chatId);
            return await _chatService.GetById(guid);
        }
        
        [HttpGet]
        public async Task<IEnumerable<ChatTO>> GetUserChats()
        {
            var userId = TokenHelpers.GetUserId(User.FindFirst(ClaimTypes.NameIdentifier)?.Value); 
            return await _chatService.GetChatsByUser(userId);
        }

        [Authorize]
        [HttpPost("assign")]
        public async Task<IActionResult> AssignUserToChat([FromQuery] string chatId)
        {  
            var userGuid = TokenHelpers.GetUserId(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var chatGuid = EntityHelpers.TryParseGuid(chatId);
            await _chatService.AssignUserToChat(userGuid, chatGuid);
            return new NoContentResult();
        }
        
        [Authorize]
        [HttpPost("unassign")]
        public async Task<IActionResult> UnassignUserToChat([FromQuery] string chatId)
        {  
            var userGuid = TokenHelpers.GetUserId(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var chatGuid = EntityHelpers.TryParseGuid(chatId);
            await _chatService.UnassignUserFromChat(userGuid, chatGuid);
            return new NoContentResult();
        }
    }
}