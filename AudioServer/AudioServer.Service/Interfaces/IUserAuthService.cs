using System;
using System.Threading.Tasks;
using AudioServer.Models.DTOs;

namespace AudioServer.Service.Interfaces
{
    public interface IUserAuthService
    {
        Task<UserTO> GetUserById(Guid userId);
        Task<TokenTO> Login(LoginTO loginTO);
        Task Register(RegisterTO registerTo);
    }
}