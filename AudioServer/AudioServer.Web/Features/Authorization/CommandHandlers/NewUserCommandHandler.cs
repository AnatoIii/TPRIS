using AudioServer.DataAccess;
using AudioServer.Models;
using AudioServer.Service.HelperFunctions;
using AudioServer.Web.Features.Authorization.Commands;
using Infrastructure.HandlerBase;
using Infrastructure.Results;
using System;
using System.Linq;

namespace AudioServer.Web.Features.Authorization.CommandHandlers
{
    /// <summary>
    /// Command handler for creating new user
    /// </summary>
    public class NewUserCommandHandler : CommandHandlerDecoratorBase<NewUserCommand, Result>
    {
        private readonly AudioServerDBContext _rDBContext;

        /// <summary>
        /// Default ctor
        /// </summary>
        /// <param name="dbContext"><see cref="AudioServerDBContext"/></param>
        public NewUserCommandHandler(AudioServerDBContext dbContext)
            : base(null)
        {
            _rDBContext = dbContext;
        }

        /// <summary>
        /// Creates <see cref="User"/>
        /// </summary>
        /// <param name="command"><see cref="NewUserCommand"/></param>
        public override void Execute(NewUserCommand command)
            => Handle(command);

        /// <summary>
        /// Handles creation of new user
        /// </summary>
        /// <param name="input">Target <see cref="NewUserCommand"/> from controkker</param>
        public override Result Handle(NewUserCommand input)
        {
            var user = _rDBContext.Users.Where(u => u.Email == input.Email).FirstOrDefault();
            if (user != null)
                return Result.Fail($"User with email `{input.Email}` already exist!");

            user = _rDBContext.Users.Where(u => u.UserName == input.Username).FirstOrDefault();
            if (user != null)
                return Result.Fail($"User with username `{input.Username}` already exist!");

            byte[] passwordSalt = PasswordHelpers.GenerateSalt();
            string passwordHash = PasswordHelpers.HashPassword(input.Password, passwordSalt);

            User newUser = new User()
            {
                UserId = Guid.NewGuid(),
                Email = input.Email,
                UserName = input.Username,
                PasswordSalt = Convert.ToBase64String(passwordSalt),
                PasswordHash = passwordHash,
            };

            _rDBContext.Users.Add(newUser);

            return Result.Ok();
        }
    }
}
