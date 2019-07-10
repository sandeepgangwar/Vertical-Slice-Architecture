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
    public class RefreshTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public RefreshTests(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldRefreshToken()
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
            var token = await _fixture.GetTokenAsync(command);
            //Arrange token request
            var request = new RefreshToken.Command()
            {
                RefreshToken = token.RefreshToken
            };
            // The endpoint or route of the controller action.
            var httpResponse = await _fixture.Client.PostAsJsonAsync("/api/tokens/refresh", request);
            _fixture.Client.DefaultRequestHeaders.Clear();

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize 
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var refreshedToken = JsonConvert.DeserializeObject<RefreshToken.Result>(stringResponse);
            var newToken = new JwtSecurityTokenHandler().ReadToken(refreshedToken.AccessToken) as JwtSecurityToken;

            // get custom claims
            var userIdFromToken = newToken.Claims.First(claim => claim.Type == "sub").Value;
            var loginFromToken = newToken.Claims.First(claim => claim.Type == "unique_name").Value;

            //get user data
            var user = _fixture.db.Users.FirstOrDefault(u => u.Id == new Guid(userIdFromToken));

            //Assert
            user.Id.ShouldBe(new Guid(userIdFromToken));
            user.Email.ShouldBe(command.Email);
            user.Login.ShouldBe(loginFromToken);

        }
    }
}
