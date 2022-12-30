using AudioServer.DataAccess;
using AudioServer.Models;
using AudioServer.Web.Features.AccountManagement.Queries;
using Infrastructure.HandlerBase;
using Infrastructure.Results;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AudioServer.Web.Features.AccountManagement.QueryHandlers
{
    /// <summary>
    /// Query handler for get a all users from <see cref="AudioServerDBContext"/>
    /// </summary>
    public class GetAllUsersQueryHandler
        : QueryHandlerDecoratorBase<GetAllUsersQuery, Result<IEnumerable<User>>>
    {
        private readonly AudioServerDBContext _rDBContext;

        /// <summary>
        /// Default ctor
        /// </summary>
        /// <param name="userSet">User <see cref="DbSet{User}"/></param>
        public GetAllUsersQueryHandler(AudioServerDBContext AudioServerDBContext)
            : base(null)
        {
            _rDBContext = AudioServerDBContext;
        }

        /// <summary>
        /// Get a certain amount of users from <see cref="AudioServerDBContext"/>
        /// </summary>
        /// <param name="userQuery"><see cref="GetAllUsersQuery"/></param>
        /// <returns><see cref="IEnumerable{User}"/></returns>
        public override Result<IEnumerable<User>> Handle(GetAllUsersQuery userQuery)
        {
            IEnumerable<User> result = _rDBContext.Users.ToList();

            if (result == null)
                return Result.Fail<IEnumerable<User>>("Problem with DB connection");

            return Result.Ok(result);
        }
    }
}
