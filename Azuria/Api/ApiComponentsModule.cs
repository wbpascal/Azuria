using Autofac;
using Azuria.Api.v1.RequestBuilder;

namespace Azuria.Api
{
    internal class ApiComponentsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            LoadRequestBuilders(builder);
        }

        private static void LoadRequestBuilders(ContainerBuilder builder)
        {
            builder.RegisterType<AnimeRequestBuilder>();
            builder.RegisterType<InfoRequestBuilder>();
            builder.RegisterType<ListRequestBuilder>();
            builder.RegisterType<MangaRequestBuilder>();
            builder.RegisterType<MediaRequestBuilder>();
            builder.RegisterType<MessengerRequestBuilder>();
            builder.RegisterType<NotificationsRequestBuilder>();
            builder.RegisterType<UcpRequestBuilder>();
            builder.RegisterType<UserRequestBuilder>();
        }
    }
}