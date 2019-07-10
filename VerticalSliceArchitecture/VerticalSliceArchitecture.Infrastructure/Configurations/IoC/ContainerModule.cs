using Autofac;
using Microsoft.Extensions.Configuration;

namespace VerticalSliceArchitecture.Infrastructure.Configurations.IoC
{
    public class ContainerModule : Module
    {
        private readonly IConfiguration _configuration;

        public ContainerModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new ServicesModule());
            builder.RegisterModule(new SettingsModule(_configuration));          
        }
    }
}