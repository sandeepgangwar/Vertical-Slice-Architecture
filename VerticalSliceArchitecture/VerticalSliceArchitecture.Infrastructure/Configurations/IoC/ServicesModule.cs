using System.Reflection;
using Autofac;
using VerticalSliceArchitecture.Core.Services;

namespace VerticalSliceArchitecture.Infrastructure.Configurations.IoC
{
    internal class ServicesModule :Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(ServicesModule).GetTypeInfo().Assembly;

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => typeof(IService)
                .IsAssignableFrom(t))
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();
        }
    }
}
