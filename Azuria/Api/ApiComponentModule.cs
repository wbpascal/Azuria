using Autofac;
using Azuria.Api.Builder;

namespace Azuria.Api
{
    internal class ApiComponentModule : Module
    {
        #region Methods

        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context => new ApiRequestBuilder(context.Resolve<IProxerClient>()))
                .As<IApiRequestBuilder>();
        }

        #endregion
    }
}