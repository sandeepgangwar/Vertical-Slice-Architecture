using Newtonsoft.Json;
using Shouldly;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VerticalSliceArchitecture.API.Features.Games;
using VerticalSliceArchitecture.API.Features.Tokens;
using VerticalSliceArchitecture.Core.Domain.Identity;
using VerticalSliceArchitecture.Tests.Integration.Utilities;
using Xunit;

namespace VerticalSliceArchitecture.Tests.Integration.Features.Games
{
    public class EditTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public EditTests(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldEditGame()
        {
            //Arrange command
            var command = new Edit.Command()
            {
                Title = "Fallout 4"
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
            var httpResponse = await _fixture.Client.PutAsJsonAsync("/api/games/1",command);
            _fixture.Client.DefaultRequestHeaders.Clear();
            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // get edited game and deserialize 
            var stringResponse = await _fixture.Client.GetAsync("/api/games/1").Result.Content.ReadAsStringAsync();
            var editedGame = JsonConvert.DeserializeObject<GetById.Result>(stringResponse);

            // Assert  
            editedGame.ShouldNotBeNull();
            editedGame.Id.ShouldBe(1);
            editedGame.Title.ShouldBe("Fallout 4");
        }
    }
    
}
