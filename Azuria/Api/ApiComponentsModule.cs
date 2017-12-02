using System.Reflection;
using Autofac;
using Azuria.Api.v1.RequestBuilder;
using Module = Autofac.Module;

namespace Azuria.Api
{
    internal class ApiComponentsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(ApiComponentsModule).GetTypeInfo().Assembly)
                .Where(type => type.IsAssignableTo<IApiClassRequestBuilder>());
        }
    }
}