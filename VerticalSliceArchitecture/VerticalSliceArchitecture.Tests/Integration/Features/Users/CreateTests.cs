using Shouldly;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VerticalSliceArchitecture.API.Features.Users;
using VerticalSliceArchitecture.Core.Domain.Identity;
using VerticalSliceArchitecture.Tests.Integration.Utilities;
using Xunit;

namespace VerticalSliceArchitecture.Tests.Integration.Features.Users
{
    public class CreateTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public CreateTests(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldRegisterUser()
        {
            //Arrange
            var command = new Create.Command()
            {
                Email = "newuser789@gmail.com",
                Login = "newUser789",
                Password = "Test123$",
                UserRole = UserRole.StandardUser,
                HasAcceptedTermsOfUse = true,
            };
            // The endpoint or route of the controller action.
            var httpResponse = await _fixture.Client.PostAsJsonAsync("/api/users", command);
            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            var addedUser = _fixture.db.Users.FirstOrDefault(u => u.Login == "newUser789");

            //Assert
            addedUser.ShouldNotBeNull();
            addedUser.Email.ShouldBe("newuser789@gmail.com");
            addedUser.Login.ShouldBe("newUser789");
        }
        
    }
}
