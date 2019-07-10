using System.Reflection;
using Autofac;
using MediatR;

namespace VerticalSliceArchitecture.API.Configurations.IoC
{
    internal class MediatrModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            builder
                .Register<ServiceFactory>(ctx =>
                {
                    var c = ctx.Resolve<IComponentContext>();
                    return t => c.TryResolve(t, out var o) ? o : null;
                })
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(MediatrModule).GetTypeInfo().Assembly).AsImplementedInterfaces();
        }
    }
}