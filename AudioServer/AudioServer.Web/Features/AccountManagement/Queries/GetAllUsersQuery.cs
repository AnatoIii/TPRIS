using AudioServer.Models;
using Infrastructure.CommandBase;
using Infrastructure.Results;
using System.Collections.Generic;

namespace AudioServer.Web.Features.AccountManagement.Queries
{
    /// <summary>
    /// Model for get methods from <see cref="AccountController.GetAll"/>
    /// </summary>
    public class GetAllUsersQuery : IQuery<Result<IEnumerable<User>>>
    { }
}
