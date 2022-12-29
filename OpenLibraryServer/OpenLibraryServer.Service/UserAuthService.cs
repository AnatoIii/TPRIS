using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenLibraryServer.DataAccess;
using OpenLibraryServer.Models;
using OpenLibraryServer.Models.DTOs;
using OpenLibraryServer.Service.Exceptions;
using OpenLibraryServer.Service.HelperFunctions;
using OpenLibraryServer.Service.Interfaces;

namespace OpenLibraryServer.Service
{
    public class UserAuthService : IUserAuthService
    {
        private readonly OpenLibraryServerDBContext _dbContext;
        private readonly TokenHelpers _tokenHelpers;

        public UserAuthService(OpenLibraryServerDBContext dbContext, TokenHelpers tokenHelpers)
        {
            _dbContext = dbContext;
            _tokenHelpers = tokenHelpers;
        }
        private TokenTO CreateTokenTO(User user)
        {
            var accessToken = _tokenHelpers.CreateJWT(user);
            var refreshTokenValue = _tokenHelpers.GenerateRefreshToken();
            return new TokenTO() { AccessToken = accessToken, RefreshToken = refreshTokenValue };
        }
        
        private Guid GetUserId(TokenTO tokenTO)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenTO.AccessToken);
            return Guid.Parse(token.Id);
        }

        public async Task<TokenTO> RefreshToken(TokenTO tokenTO)
        {
            var userId = GetUserId(tokenTO);
            var user = await GetDBUser(userId);
            var token = await _dbContext.Tokens.FirstOrDefaultAsync(t => t.RefreshToken == tokenTO.RefreshToken);
            _dbContext.Tokens.Remove(token);

            var newToken = CreateTokenTO(user);
            var dbToken = new Token()
            {
                RefreshToken = newToken.RefreshToken,
                User = user,
                DueDate = DateTime.Now.AddMinutes(_tokenHelpers.GetTokenConfig().RefreshTokenLifetime)
            };
            await _dbContext.Tokens.AddAsync(dbToken);
            await _dbContext.SaveChangesAsync();
            return newToken;
        }
        
        public async Task<UserTO> GetUserById(Guid userId)
        {
            var user = await GetDBUser(userId);
            return new UserTO()
            {
                UserId = user.UserId,
                UserName = user.UserName,
                UITheme = user.UITheme
            };
        }

        private async Task<User> GetDBUser(Guid userId)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                throw new NotFoundException($"User with Id {userId} not found");
            }

            return user;
        }

        public async Task<TokenTO> Login(LoginTO loginTO)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == loginTO.UserName);
            if(user == null)
                throw new BadRequestException($"User with username {loginTO.UserName} not found");

            if(!PasswordHelpers.ValidatePassword(loginTO.Password, user.PasswordSalt, user.PasswordHash))
                throw new BadRequestException("Invalid password");

            var tokenTO = CreateTokenTO(user);
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

        public async Task ChangeUserPassword(ChangePasswordTO changePasswordTo, Guid userId)
        {
            var user = await _dbContext.Users.Where(u => u.UserId == userId).FirstAsync();
            if(!PasswordHelpers.ValidatePassword(changePasswordTo.OldPassword, user.PasswordSalt, user.PasswordHash))
                throw new BadRequestException("Invalid old password");
            user.PasswordHash = PasswordHelpers.HashPassword(changePasswordTo.NewPassword, Encoding.UTF8.GetBytes(user.PasswordSalt));
            await _dbContext.SaveChangesAsync();
        }

        public async Task ChangeUserTheme(ChangeThemeTO changeThemeTo, Guid userId)
        {
            var user = await GetDBUser(userId);
            user.UITheme = changeThemeTo.Theme;
            await _dbContext.SaveChangesAsync();
        }
    }
}