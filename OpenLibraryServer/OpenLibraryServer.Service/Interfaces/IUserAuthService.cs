using System;
using System.Threading.Tasks;
using OpenLibraryServer.Models.DTOs;

namespace OpenLibraryServer.Service.Interfaces
{
    public interface IUserAuthService
    {
        Task<UserTO> GetUserById(Guid userId);
        Task<TokenTO> Login(LoginTO loginTO);
        Task Register(RegisterTO registerTo);
        Task ChangeUserPassword(ChangePasswordTO changePasswordTo, Guid userId);
        Task ChangeUserTheme(ChangeThemeTO changeThemeTo, Guid userId);
    }
}