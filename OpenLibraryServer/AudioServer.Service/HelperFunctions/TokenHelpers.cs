using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AudioServer.Models;
using AudioServer.Service.Exceptions;

namespace AudioServer.Service.HelperFunctions
{
    public class TokenHelpers
    {
        private readonly TokenConfig _tokenConfig;
        private readonly SigningCredentials _signingCredentials;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        
        public TokenHelpers(IOptions<TokenConfig> tokenConfig)
        {
            _tokenConfig = tokenConfig.Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.Secret));
            _signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public TokenConfig GetTokenConfig()
        {
            return _tokenConfig;
        }
        
        public string CreateJWT(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _tokenConfig.Issuer,
                audience: _tokenConfig.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_tokenConfig.JWTLifetime),
                signingCredentials: _signingCredentials
            );

            return _tokenHandler.WriteToken(token);
        }
        
        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
        
        public static Guid GetUserId(string token)
        {
            if (token == null)
            {
                throw new UnauthorizedException("Empty id from token");
            }
            return EntityHelpers.TryParseGuid(token);
        }
    }
}