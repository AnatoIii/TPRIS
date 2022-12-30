using System;

namespace AudioServer.Web.Features.Authorization
{
    /// <summary>
    /// Transfer object for auth <see cref="AuthController"/>
    /// </summary>
    public class TokenDTO
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}
