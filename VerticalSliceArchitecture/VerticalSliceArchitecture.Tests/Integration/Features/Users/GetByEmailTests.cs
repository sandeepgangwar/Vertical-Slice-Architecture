using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VerticalSliceArchitecture.API.Features.Users;
using VerticalSliceArchitecture.Infrastructure.Exceptions;
using VerticalSliceArchitecture.Tests.Integration.Utilities;
using Xunit;

namespace VerticalSliceArchitecture.Tests.Integration.Features.Users
{
    public class GetByEmailTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public GetByEmailTests(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldGetUserByEmail()
        {

            // The endpoint or route of the controller action.
            var httpResponse = await _fixture.Client.GetAsync("/api/users/user123@gmail.com");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<GetByEmail.Result>(stringResponse);

            // Assert   
            user.Email.ShouldBe("user123@gmail.com");
            user.Login.ShouldBe("user123");

        }
       
        
    }
}
