using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using VerticalSliceArchitecture.API;
using VerticalSliceArchitecture.API.Features.Tokens;
using VerticalSliceArchitecture.Infrastructure.Data;

namespace VerticalSliceArchitecture.Tests.Integration.Utilities
{
    public class TestServerFixture : IDisposable
    {
        private readonly TestServer _testServer;
        public readonly HttpClient Client;
        public readonly AppDb db;
        public IHostingEnvironment CurrentEnvironment { get; }

        public TestServerFixture()
        {

            string appRootPath = Path.GetFullPath(Path.Combine(
                           AppContext.BaseDirectory,
                           "..", "..", "..", "..", "VerticalSliceArchitecture.API"));
            string appSettingsPath = Path.Combine(appRootPath, "appsettings.json");

            var builder = new WebHostBuilder()
            .UseEnvironment("Test")
         // .UseEnvironment("Development") to use Default connection DB
            .UseContentRoot(appRootPath)
            .UseConfiguration(new ConfigurationBuilder().AddJsonFile(appSettingsPath).Build())
            .UseStartup<Startup>();

            _testServer = new TestServer(builder);
            Client = _testServer.CreateClient();
            db = _testServer.Host.Services.GetService(typeof(AppDb)) as AppDb;
            CurrentEnvironment = new HostingEnvironment();

            if (db.Database.EnsureCreated())
            {
                 SeedData.PopulateTestData(db);               
            }

        }
        /// <summary>
        /// gets auth token if provided data from command is valid
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<CreateToken.Result> GetTokenAsync(CreateToken.Command command)
        {
            var httpResponse = await Client.PostAsJsonAsync("/api/tokens", command);
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<CreateToken.Result>(stringResponse);
        }

        public void Dispose()
        {
            Client.Dispose();
            _testServer.Dispose();
            db.Dispose();
        }
    }
}
