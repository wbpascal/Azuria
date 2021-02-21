using System;
using Azuria.Api.v1.DataModels.Notifications;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converter.Notifications
{
    internal class NotificationCountConverter : DataConverter<NotificationCountDataModel>
    {
        /// <inheritdoc />
        public override NotificationCountDataModel ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var lDataModel = new NotificationCountDataModel();
            int? lValue;
            for (var i = 0; (lValue = reader.ReadAsInt32()) != null; i++)
            {
                if (reader.TokenType == JsonToken.EndArray) break;
                switch (i)
                {
                    case 2:
                        lDataModel.PrivateMessages = lValue.Value;
                        break;
                    case 3:
                        lDataModel.FriendRequests = lValue.Value;
                        break;
                    case 4:
                        lDataModel.News = lValue.Value;
                        break;
                    case 5:
                        lDataModel.OtherMedia = lValue.Value;
                        break;
                }
            }

            return lDataModel;
        }
    }
}