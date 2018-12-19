using Xunit;
using Rawdata.Service.Models;
using Rawdata.Service.Controllers;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Rawdata.Data;

namespace Rawdata.Tests
{
    public class AuthControllerTests : Setup
    {
        private readonly AuthController AuthController;

        public AuthControllerTests(ITestOutputHelper output) : base(output)
        {
            AuthController = new AuthController(DtoMapper, UserService) {
                Url = UrlHelper,
                ControllerContext = CreateControllerContext()
            };
        }

        [Fact]
        public async Task RegisterUser_Should_Succeed()
        {
            var userDto = new UserRegisterDto {
                DisplayName = "Test User",
                Email = "unit_test@dev.local",
                Password = "123456789"
            };

            var registerResult = await AuthController
                .RegisterUser(userDto);

            Assert.True(AuthController.ModelState.IsValid);

            var registerStatus = registerResult
                .GetType()
                .GetProperty("StatusCode")
                .GetValue(registerResult, null) as int?;

            var user = await UserService.GetUserByEmail(userDto.Email);

            Assert.Equal(user.DisplayName, userDto.DisplayName);
            Assert.Equal(user.Email, userDto.Email);
            Assert.True(PasswordHelper.Validate(userDto.Password, user.Password));
            Assert.NotEqual(500, registerStatus);
        }
    }
}
