using Shouldly;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VerticalSliceArchitecture.API.Features.Games;
using VerticalSliceArchitecture.API.Features.Tokens;
using VerticalSliceArchitecture.Core.Domain.Identity;
using VerticalSliceArchitecture.Core.Helpers;
using VerticalSliceArchitecture.Tests.Integration.Utilities;
using Xunit;

namespace VerticalSliceArchitecture.Tests.Integration.Features.Games
{
   public class CreateTests : IClassFixture<TestServerFixture>
   {
        private readonly TestServerFixture _fixture;

        public CreateTests(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldAddNewGame()
        {
            //Arrange command
            var command = new Create.Command()
            {
                Title = "Fallout 4",
                ReleaseDate = DateTimeHelper.Now
            };
            //Arrange token
            var request = new CreateToken.Command()
            {
                Email = "user123@gmail.com",
                Password = "Test123$",
                DeviceId = "SMSNG45543",
                DeviceName = "DEVICE",
                Platform = Platform.Web
            };
            var token = await _fixture.GetTokenAsync(request);

            _fixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            // The endpoint or route of the controller action.
            var httpResponse = await _fixture.Client.PostAsJsonAsync("/api/games", command);
            _fixture.Client.DefaultRequestHeaders.Clear();
            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            var addedGame = _fixture.db.Games.FirstOrDefault(u => u.Title == "Fallout 4");

            //Assert
            addedGame.ShouldNotBeNull();
            addedGame.Title.ShouldBe("Fallout 4");

        }
    }
}
