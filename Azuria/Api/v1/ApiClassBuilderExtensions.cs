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
            return new AnimeRequestBuilder(builder.ProxerClient);
        }

        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static InfoRequestBuilder FromInfoClass(this IApiRequestBuilder builder)
        {
            return new InfoRequestBuilder(builder.ProxerClient);
        }

        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ListRequestBuilder FromListClass(this IApiRequestBuilder builder)
        {
            return new ListRequestBuilder(builder.ProxerClient);
        }

        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static MangaRequestBuilder FromMangaClass(this IApiRequestBuilder builder)
        {
            return new MangaRequestBuilder(builder.ProxerClient);
        }

        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static MediaRequestBuilder FromMediaClass(this IApiRequestBuilder builder)
        {
            return new MediaRequestBuilder(builder.ProxerClient);
        }

        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static MessengerRequestBuilder FromMessengerClass(this IApiRequestBuilder builder)
        {
            return new MessengerRequestBuilder(builder.ProxerClient);
        }

        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static NotificationsRequestBuilder FromNotificationClass(this IApiRequestBuilder builder)
        {
            return new NotificationsRequestBuilder(builder.ProxerClient);
        }

        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static UcpRequestBuilder FromUcpClass(this IApiRequestBuilder builder)
        {
            return new UcpRequestBuilder(builder.ProxerClient);
        }

        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static UserRequestBuilder FromUserClass(this IApiRequestBuilder builder)
        {
            return new UserRequestBuilder(builder.ProxerClient);
        }
    }
}