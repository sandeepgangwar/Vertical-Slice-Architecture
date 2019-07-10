using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Autofac;
using VerticalSliceArchitecture.Infrastructure.Configurations.App;
using VerticalSliceArchitecture.Infrastructure.Extensions;

namespace VerticalSliceArchitecture.Infrastructure.Configurations.IoC
{
    internal class SettingsModule : Autofac.Module
    {
        private readonly IConfiguration _configuration;

        public SettingsModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(SettingsModule)
                .GetTypeInfo()
                .Assembly;

            var types = assembly.GetTypes()
                .Where(t => (typeof(ISettings)).IsAssignableFrom(t) && !t.IsInterface)
                .ToList();

            types.ForEach(t =>
            {
                builder.RegisterInstance(_configuration.GetSettings(t))
                    .As(t)
                    .SingleInstance();
            });
        }
    }
}