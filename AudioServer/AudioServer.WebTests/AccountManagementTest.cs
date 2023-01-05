using AudioServer.DataAccess;
using AudioServer.Models;
using AudioServer.Service.HelperFunctions;
using AudioServer.Web.Features.AccountManagement;
using AudioServer.Web.Features.AccountManagement.Queries;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using TestsInfrastructure;

namespace AudioServer.WebTests
{
    public class AccountManagementTest
    {
        #region Test classes

        private readonly TestLogger<AccountController> _rTestLogger;
        private readonly AudioServerDBContext _rDbContext;
        private readonly List<User> _testData;

        #endregion

        public AccountManagementTest()
        {
            _rTestLogger = new TestLogger<AccountController>();

            _testData = _GetTestableUserData();

            _rDbContext = DbHelper.GetTestContextWithTargetParams("User Test", _testData);
        }

        #region GetAllTests

        [Fact]
        [Trait("Category", "IntegrationTests")]
        [Trait("Category", "AccountController")]
        [Trait("Category", "GetAll")]
        public void AccountController_GetAll_With_Some_Data_Works_Fine()
        {
            AccountController accountController = new AccountController(_rDbContext, _rTestLogger);

            _rTestLogger.LoggedMessages.Should().BeEmpty();

            var response = accountController.GetAll();

            _rTestLogger.LoggedMessages.Should().NotBeEmpty()
                .And.HaveCount(1);

            var okObjectResult = response as OkObjectResult;
            okObjectResult.Should().NotBeNull();

            okObjectResult.StatusCode.Should().NotBeNull()
                .And.Be(200);

            List<User> result = okObjectResult.Value as List<User>;

            result.Should().NotBeNull()
                .And.HaveCount(_testData.Count)
                .And.BeEquivalentTo(_testData);
        }

        [Fact]
        [Trait("Category", "IntegrationTests")]
        [Trait("Category", "AccountController")]
        [Trait("Category", "GetUser")]
        public void AccountController_GetUser_That_Exists_Works_Fine()
        {
            AccountController accountController = new AccountController(_rDbContext, _rTestLogger);

            GetUserQuery getUserQuery = new GetUserQuery()
            {
                UserId = _testData[0].UserId
            };

            _rTestLogger.LoggedMessages.Should().BeEmpty();

            var response = accountController.GetUser(getUserQuery);

            _rTestLogger.LoggedMessages.Should().NotBeEmpty()
                .And.HaveCount(1);

            var okObjectResult = response as OkObjectResult;
            okObjectResult.Should().NotBeNull();

            okObjectResult.StatusCode.Should().NotBeNull()
                .And.Be(200);

            User result = okObjectResult.Value as User;

            result.Should().NotBeNull()
                .And.BeEquivalentTo(_testData[0]);
        }

        #endregion

        #region GetUserTests

        [Fact]
        [Trait("Category", "IntegrationTests")]
        [Trait("Category", "AccountController")]
        [Trait("Category", "GetUser")]
        public void AccountController_GetUser_Empty_Guid_Works_Fine()
        {
            AccountController accountController = new AccountController(_rDbContext, _rTestLogger);

            GetUserQuery getUserQuery = new GetUserQuery()
            {
                UserId = Guid.Empty
            };

            _rTestLogger.LoggedMessages.Should().BeEmpty();

            var response = accountController.GetUser(getUserQuery);

            _rTestLogger.LoggedMessages.Should().NotBeEmpty()
                .And.HaveCount(1);

            var badRequestObjectResult = response as BadRequestObjectResult;
            badRequestObjectResult.Should().NotBeNull();

            badRequestObjectResult.StatusCode.Should().NotBeNull()
                .And.Be(400);

            string result = badRequestObjectResult.Value.ToString();

            result.Should().NotBeNull()
                .And.Be($"{nameof(getUserQuery)} failed. Message: User not found. User can`t be with empty id.");
        }

        [Fact]
        [Trait("Category", "IntegrationTests")]
        [Trait("Category", "AccountController")]
        [Trait("Category", "GetUser")]
        public void AccountController_GetUser_User_Not_Found_Works_Fine()
        {
            AccountController accountController = new AccountController(_rDbContext, _rTestLogger);

            Guid testId = Guid.NewGuid();
            GetUserQuery getUserQuery = new GetUserQuery()
            {
                UserId = testId
            };

            _rTestLogger.LoggedMessages.Should().BeEmpty();

            var response = accountController.GetUser(getUserQuery);

            _rTestLogger.LoggedMessages.Should().NotBeEmpty()
                .And.HaveCount(1);

            var badRequestObjectResult = response as BadRequestObjectResult;
            badRequestObjectResult.Should().NotBeNull();

            badRequestObjectResult.StatusCode.Should().NotBeNull()
                .And.Be(400);

            string result = badRequestObjectResult.Value.ToString();

            result.Should().NotBeNull()
                .And.Be($"{nameof(getUserQuery)} failed. Message: User not found. User with id {testId} not found.");
        }

        [Fact]
        [Trait("Category", "IntegrationTests")]
        [Trait("Category", "AccountController")]
        [Trait("Category", "GetUser")]
        public void AccountController_GetUser_User_Id_Cant_Be_Empty_Works_Fine()
        {
            AccountController accountController = new AccountController(_rDbContext, _rTestLogger);

            GetUserQuery getUserQuery = new GetUserQuery()
            {
                UserId = Guid.Empty
            };

            _rTestLogger.LoggedMessages.Should().BeEmpty();

            var response = accountController.GetUser(getUserQuery);

            _rTestLogger.LoggedMessages.Should().NotBeEmpty()
                .And.HaveCount(1);

            var badRequestObjectResult = response as BadRequestObjectResult;
            badRequestObjectResult.Should().NotBeNull();

            badRequestObjectResult.StatusCode.Should().NotBeNull()
                .And.Be(400);

            string result = badRequestObjectResult.Value.ToString();

            result.Should().NotBeNull()
                .And.Be($"{nameof(getUserQuery)} failed. Message: User not found. User can`t be with empty id.");
        }

        #endregion

        private List<User> _GetTestableUserData()
        {
            var data = new List<User>();

            byte[] passwordSalt = PasswordHelpers.GenerateSalt();
            string passwordHash = PasswordHelpers.HashPassword("Testpass1", passwordSalt);

            data.Add(new User()
            {
                UserId = Guid.NewGuid(),
                UserName = "FNTest1",
                Email = "TestEmail1",
                PasswordHash = passwordHash,
                PasswordSalt = Convert.ToBase64String(passwordSalt)
            });

            passwordSalt = PasswordHelpers.GenerateSalt();
            passwordHash = PasswordHelpers.HashPassword("Testpass2", passwordSalt);

            data.Add(new User()
            {
                UserId = Guid.NewGuid(),
                UserName = "FNTest2",
                Email = "TestEmail2",
                PasswordHash = passwordHash,
                PasswordSalt = Convert.ToBase64String(passwordSalt)
            });

            return data;
        }
    }
}