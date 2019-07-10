using Newtonsoft.Json;
using Shouldly;
using System.Threading.Tasks;
using VerticalSliceArchitecture.API.Features.Games;
using VerticalSliceArchitecture.Tests.Integration.Utilities;
using Xunit;

namespace VerticalSliceArchitecture.Tests.Integration.Features.Games
{
    public class GetByIdTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public GetByIdTests(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldGetGameById()
        {

            // The endpoint or route of the controller action.
            var httpResponse = await _fixture.Client.GetAsync("/api/games/1");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var game = JsonConvert.DeserializeObject<GetById.Result>(stringResponse);

            // Assert   
            game.Id.ShouldBe(1);
            game.Title.ShouldBe("Final Fantasy");

        }
    }
}
