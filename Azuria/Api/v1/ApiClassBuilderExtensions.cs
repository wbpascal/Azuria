using Autofac;
using Azuria.Api.Builder;
using Azuria.Api.v1.RequestBuilder;

namespace Azuria.Api.v1
{
    /// <summary>
    /// </summary>
    public static class ApiClassBuilderExtensions
    {
        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static AnimeRequestBuilder FromAnimeClass(this IApiRequestBuilder builder)
        {
            return builder.ProxerClient.Container.Resolve<AnimeRequestBuilder>();
        }

        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static InfoRequestBuilder FromInfoClass(this IApiRequestBuilder builder)
        {
            return builder.ProxerClient.Container.Resolve<InfoRequestBuilder>();
        }

        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ListRequestBuilder FromListClass(this IApiRequestBuilder builder)
        {
            return builder.ProxerClient.Container.Resolve<ListRequestBuilder>();
        }

        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static MangaRequestBuilder FromMangaClass(this IApiRequestBuilder builder)
        {
            return builder.ProxerClient.Container.Resolve<MangaRequestBuilder>();
        }

        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static MediaRequestBuilder FromMediaClass(this IApiRequestBuilder builder)
        {
            return builder.ProxerClient.Container.Resolve<MediaRequestBuilder>();
        }

        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static MessengerRequestBuilder FromMessengerClass(this IApiRequestBuilder builder)
        {
            return builder.ProxerClient.Container.Resolve<MessengerRequestBuilder>();
        }

        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static NotificationsRequestBuilder FromNotificationClass(this IApiRequestBuilder builder)
        {
            return builder.ProxerClient.Container.Resolve<NotificationsRequestBuilder>();
        }

        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static UcpRequestBuilder FromUcpClass(this IApiRequestBuilder builder)
        {
            return builder.ProxerClient.Container.Resolve<UcpRequestBuilder>();
        }

        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static UserRequestBuilder FromUserClass(this IApiRequestBuilder builder)
        {
            return builder.ProxerClient.Container.Resolve<UserRequestBuilder>();
        }
    }
}