using Autofac;
using Microsoft.Extensions.Configuration;
using VerticalSliceArchitecture.API.Configurations.Mappings;

namespace VerticalSliceArchitecture.API.Configurations.IoC
{
    public class ContainerModule : Module
    {
        private readonly IConfiguration _configuratiion;

        public ContainerModule(IConfiguration configuratiion)
        {
            _configuratiion = configuratiion;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(AutoMapperConfig.Initialize()).SingleInstance();
            builder.RegisterModule(new MediatrModule());
        }
    }
}
