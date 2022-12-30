using AudioServer.DataAccess;
using AudioServer.Web.Features.Authorization.CommandHandlers;
using AudioServer.Web.Features.Authorization.Commands;
using Infrastructure.DecoratorsFactory;
using Infrastructure.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AudioServer.Web.Features.Authorization
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AudioServerDBContext _dbContext;
        private readonly ILogger<AuthController> _logger;
        private readonly TokenCreator _tokenCreator;

        public AuthController(AudioServerDBContext dbContext, ILogger<AuthController> logger, TokenCreator tokenCreator)
        {
            _dbContext = dbContext;
            _logger = logger;
            _tokenCreator = tokenCreator;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginCommand loginCommand)
        {
            var handler = new CommandDecoratorBuilder<LoginCommand, Result<TokenDTO>>()
                .Add<LoginCommandHandler>()
                    .AddParameter<AudioServerDBContext>(_dbContext)
                    .AddParameter<TokenCreator>(_tokenCreator)
                .AddBaseDecorators(_logger, _dbContext)
                .Build();

            var result = handler.Handle(loginCommand);

            if (result.Failure)
                return BadRequest($"{nameof(loginCommand)} failed. Message: {result.Error}");

            return Ok(result.Value);
        }

        [HttpPost("create")]
        public IActionResult CreateNewUser([FromBody] NewUserCommand newUserCommand)
        {
            var handler = new CommandDecoratorBuilder<NewUserCommand, Result>()
                .Add<NewUserCommandHandler>()
                    .AddParameter<AudioServerDBContext>(_dbContext)
                .AddBaseDecorators(_logger, _dbContext)
                .Build();

            var result = handler.Handle(newUserCommand);

            if (result.Failure)
                return BadRequest($"{nameof(newUserCommand)} failed. Message: {result.Error}");

            return Ok();
        }
    }
}