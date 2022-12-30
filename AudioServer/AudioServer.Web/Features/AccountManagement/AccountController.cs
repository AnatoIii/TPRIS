using AudioServer.DataAccess;
using AudioServer.Models;
using AudioServer.Web.Features.AccountManagement.Queries;
using AudioServer.Web.Features.AccountManagement.QueryHandlers;
using Infrastructure.DecoratorsFactory;
using Infrastructure.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace AudioServer.Web.Features.AccountManagement
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AudioServerDBContext _rDBContext;
        private readonly ILogger<AccountController> _rLogger;

        public AccountController(AudioServerDBContext dbContext,
            ILogger<AccountController> logger)
        {
            _rDBContext = dbContext;
            _rLogger = logger;
        }

        [HttpGet()]
        public IActionResult GetUser([FromQuery] GetUserQuery getUserQuery)
        {
            var handler = new QueryDecoratorBuilder<GetUserQuery, Result<User>>()
                .Add<GetUserQueryHandler>()
                    .AddParameter<AudioServerDBContext>(_rDBContext)
                .AddBaseDecorators(_rLogger)
                .Build();

            var result = handler.Handle(getUserQuery);

            if (result.Failure)
                return BadRequest($"{nameof(getUserQuery)} failed. Message: {result.Error}");

            return Ok(result.Value);
        }

        [HttpGet(), Route("getAll")]
        public IActionResult GetAll()
        {
            var handler = new QueryDecoratorBuilder<GetAllUsersQuery, Result<IEnumerable<User>>>()
                .Add<GetAllUsersQueryHandler>()
                    .AddParameter<AudioServerDBContext>(_rDBContext)
                .AddBaseDecorators(_rLogger)
                .Build();

            var getAllUsersQuery = new GetAllUsersQuery();

            var result = handler.Handle(getAllUsersQuery);

            if (result.Failure)
                return BadRequest($"{nameof(getAllUsersQuery)} failed. Message: {result.Error}");

            return Ok(result.Value);
        }
    }
}
