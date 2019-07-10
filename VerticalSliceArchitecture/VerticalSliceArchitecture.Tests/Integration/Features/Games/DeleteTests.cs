using Shouldly;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VerticalSliceArchitecture.API.Features.Tokens;
using VerticalSliceArchitecture.Core.Domain.Identity;
using VerticalSliceArchitecture.Tests.Integration.Utilities;
using Xunit;

namespace VerticalSliceArchitecture.Tests.Integration.Features.Games
{
    public class DeleteTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public DeleteTests(TestServerFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        public async Task ShouldDeleteGame()
        {           
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
            var httpResponse = await _fixture.Client.DeleteAsync("/api/games/1");
            _fixture.Client.DefaultRequestHeaders.Clear();
            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            var deletedGame = _fixture.db.Games.FirstOrDefault(g => g.Id == 1);

            //Assert
            deletedGame.ShouldBeNull();

        }
    }
}
