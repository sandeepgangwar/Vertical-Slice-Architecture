using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VerticalSliceArchitecture.API.Features.Games;
using VerticalSliceArchitecture.Tests.Integration.Utilities;
using Xunit;

namespace VerticalSliceArchitecture.Tests.Integration.Features.Games
{
    public class GetAllTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public GetAllTests(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldGetAllGames()
        {
            // The endpoint or route of the controller action.
            var httpResponse = await _fixture.Client.GetAsync("/api/games");
            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetAll.Result>(stringResponse);

            //Assert
            result.Games.ShouldNotBeNull();
            result.Games.Count().ShouldBe(3);
            result.Games.ShouldContain(p => p.Title == "Final Fantasy");
            result.Games.ShouldContain(p => p.Title == "Metal Gear Solid");
            result.Games.ShouldContain(p => p.Title == "Metal Slug");

        }
           
    }
}
