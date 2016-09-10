using Azuria.Api.v1.Converters.Notifications;
using Azuria.Api.v1.DataModels.Notifications;
using Newtonsoft.Json;

namespace Azuria.Api.v1
{
    internal class ProxerNotificationCountResponse : ProxerApiResponse
    {
        #region Properties

        [JsonProperty("data")]
        [JsonConverter(typeof(NotificationCountConverter))]
        internal NotificationCountDataModel Data { get; set; }

        #endregion
    }
}