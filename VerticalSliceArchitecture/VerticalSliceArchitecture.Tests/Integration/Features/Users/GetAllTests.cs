using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VerticalSliceArchitecture.API.Features.Tokens;
using VerticalSliceArchitecture.API.Features.Users;
using VerticalSliceArchitecture.Core.Domain.Identity;
using VerticalSliceArchitecture.Tests.Integration.Utilities;
using Xunit;

namespace VerticalSliceArchitecture.Tests.Integration.Features.Users
{
    public class GetAllTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public GetAllTests(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldGetAllUsers()
        {
            var command = new CreateToken.Command()
            {
                Email = "admin789@gmail.com",
                Password = "Test123$",
                DeviceId = "SMSNG45543",
                DeviceName = "DEVICE",
                Platform = Platform.Web
            };
            var token = await _fixture.GetTokenAsync(command);

            _fixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            // The endpoint or route of the controller action.
            var httpResponse = await _fixture.Client.GetAsync("/api/users");
            _fixture.Client.DefaultRequestHeaders.Clear();
            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<GetAll.Result>(stringResponse);

            //Assert
            result.Users.ShouldNotBeNull();
            result.Users.Count().ShouldBe(3);
            result.Users.ShouldContain(p => p.Email == "user123@gmail.com");
            result.Users.ShouldContain(p => p.Email == "user456@gmail.com");
            result.Users.ShouldContain(p => p.Email == "admin789@gmail.com");

        }
    }
    
}
