using Newtonsoft.Json;
using Shouldly;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VerticalSliceArchitecture.API.Features.Tokens;
using VerticalSliceArchitecture.Core.Domain.Identity;
using VerticalSliceArchitecture.Tests.Integration.Utilities;
using Xunit;

namespace VerticalSliceArchitecture.Tests.Integration.Features.Tokens
{
    public class CreateTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public CreateTests(TestServerFixture fixture)
        {
            _fixture = fixture;
        }
                       
        [Fact]
        public async Task ShouldGetToken()
        {
            //Arrange command
            var command = new CreateToken.Command()
            {
                Email = "user123@gmail.com",
                Password = "Test123$",
                DeviceId = "SMSNG45543",
                DeviceName = "DEVICE",
                Platform = Platform.Web
            };
            // The endpoint or route of the controller action.
            var httpResponse = await _fixture.Client.PostAsJsonAsync("/api/tokens", command);
            _fixture.Client.DefaultRequestHeaders.Clear();

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize 
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<CreateToken.Result>(stringResponse);
            var accessToken = new JwtSecurityTokenHandler().ReadToken(token.AccessToken) as JwtSecurityToken;

            // get custom claims
            var userIdFromToken = accessToken.Claims.First(claim => claim.Type == "sub").Value;
            var loginFromToken = accessToken.Claims.First(claim => claim.Type == "unique_name").Value;

            //get user data
            var user = _fixture.db.Users.FirstOrDefault(u => u.Id == new Guid(userIdFromToken));

            //Assert
            user.Id.ShouldBe(new Guid(userIdFromToken));
            user.Email.ShouldBe(command.Email);
            user.Login.ShouldBe(loginFromToken);

        }
       
    }
}
