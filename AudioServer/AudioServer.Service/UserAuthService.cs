using AudioServer.DataAccess;
using AudioServer.Models;
using AudioServer.Models.DTOs;
using AudioServer.Service.Exceptions;
using AudioServer.Service.HelperFunctions;
using AudioServer.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AudioServer.Service
{
    public class UserAuthService : IUserAuthService
    {
        private readonly AudioServerDBContext _dbContext;
        private readonly TokenHelpers _tokenHelpers;

        public UserAuthService(AudioServerDBContext dbContext, TokenHelpers tokenHelpers)
        {
            _dbContext = dbContext;
            _tokenHelpers = tokenHelpers;
        }
        
        public async Task<UserTO> GetUserById(Guid userId)
        {
            var user = await _GetDBUser(userId);

            return new UserTO()
            {
                UserId = user.UserId,
                UserName = user.UserName
            };
        }

        public async Task<TokenTO> Login(LoginTO loginTO)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == loginTO.UserName);
            if(user == null)
                throw new BadRequestException($"User with username {loginTO.UserName} not found");

            if(!PasswordHelpers.ValidatePassword(loginTO.Password, user.PasswordSalt, user.PasswordHash))
                throw new BadRequestException("Invalid password");

            var tokenTO = _CreateTokenTO(user);
            var dbToken = new Token()
            {
                RefreshToken = tokenTO.RefreshToken,
                User = user,
                DueDate = DateTime.Now.AddMinutes(_tokenHelpers.GetTokenConfig().RefreshTokenLifetime)
            };

            await _dbContext.Tokens.AddAsync(dbToken);
            await _dbContext.SaveChangesAsync();

            return tokenTO;
        }

        public async Task Register(RegisterTO registerTo)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == registerTo.UserName);
            if (user != null)
                throw new AlreadyExistsException($"User with username {registerTo.UserName} already exists");

            byte[] passwordSalt = PasswordHelpers.GenerateSalt();
            string passwordHash = PasswordHelpers.HashPassword(registerTo.Password, passwordSalt);

            User newUser = new User()
            {
                UserName = registerTo.UserName,
                PasswordSalt = Convert.ToBase64String(passwordSalt),
                PasswordHash = passwordHash
            };

            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
        }

        private TokenTO _CreateTokenTO(User user)
        {
            var accessToken = _tokenHelpers.CreateJWT(user);
            var refreshTokenValue = _tokenHelpers.GenerateRefreshToken();

            return new TokenTO() { AccessToken = accessToken, RefreshToken = refreshTokenValue };
        }

        private async Task<User> _GetDBUser(Guid userId)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
                throw new NotFoundException($"User with Id {userId} not found");

            return user;
        }
    }
}